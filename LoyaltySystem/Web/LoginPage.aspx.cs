using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginPage : System.Web.UI.Page
{
    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //  private string port = WebConfigurationManager.AppSettings["API_PORT"];
    private string successAuth = "KeepMeLoggedIn";

    protected void Page_Load(object sender, EventArgs e)
    {
        init();
    }

    //Init Method
    private void init()
    {
        DivFailed.Visible = false;
        DivFailedNewComp.Visible = false;
        DivConnectionErr.Visible = false;
    }

    //---------------------------------------------------------------> Log In Stuff Below <----------------------------------------------------------

    //Get a new token from AUTH0 and pass it in as a parameter to checkLoginDetails
    protected void singInBtn_Click(object sender, EventArgs e)
    {
        try
        {
            checkLoginDetails(Authorization.GetAuth());
        }
        catch (Exception)
        {
            DivConnectionErr.Visible = true;
        }
    }


    /*Method Checks the log in details of the user input against the database via The API. 
    Passing in the Auth0 Token etc*/
    private void checkLoginDetails(Token auth)
    {
        var client = new RestClient(port);
        UserSettings settings = new UserSettings();

        var request = new RestRequest("api/authenticate", Method.POST);
        request.AddParameter("email", TbEmail.Text); //email fro Textbox
        request.AddParameter("password", TbPassword.Text); //pwd from Textbox
        request.AddParameter("type", "business");
        request.AddHeader("Authorization", auth.token_type + " " + auth.access_token);

        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<BizCred>(response.Content);
        var bizObj = jsonObject as BizCred;

        //If Success
        if (bizObj.success == true)
        {
            // ------------------------ SET COOKIE NOT WORKING 100% ------------------------  
            #region
            //System.Web.HttpCookie newCookie = new System.Web.HttpCookie("BIZ_DETAILS");
            //newCookie.Values.Add("BIZ_NAME", bizObj.name);
            //newCookie.Values.Add("AUTH_SUCC", bizObj.success.ToString());
            //newCookie.Secure = true;
            //newCookie.Name = "BIZ_DETAILS";
            //newCookie.Expires = DateTime.Now.AddMinutes(10.0);
            //newCookie.Path = "/";

            //Response.Cookies.Add(newCookie);
            #endregion

            // ------------------------ Create Settings Obj ------------------------ 
            settings._auth_Token = Encrypt.Base64Encode(auth.access_token);
            settings._biz_Name = Encrypt.Base64Encode(bizObj.name);
            settings._auth_Type = Encrypt.Base64Encode(auth.token_type);
            settings._loggedIn= Encrypt.Base64Encode(successAuth);
            settings._biz_Email = Encrypt.Base64Encode(TbEmail.Text);
            
            // ------------------------ TEMP CACHE KEYS ETC Encrypted ------------------------ 
            Cache["Settings"] = settings;

            //Successful Login
            Response.Redirect("Default.aspx", true);
        }
        else
        {
            DivFailed.Visible = true;
        }
    }


    //---------------------------------------------------------------> Register Company Page Stuff Below <----------------------------------------------------------

    //Btn Click event calls nessassary Methods 
    protected void BtnCheckPersonIn_Click(object sender, EventArgs e)
    {
        Company comp = NewCompanyObject();
        CreateNewCompany(Authorization.GetAuth(), comp);
    }

    //Create a new Company Object
    private Company NewCompanyObject()
    {
        Company comp = new Company();
        comp.name = TbName.Text.ToString();
        comp.email = TbEmailNew.Text.ToString();
        comp.phone = TbContactNum.Text.ToString();
        comp.address = TbAddress.Text.ToString();
        comp.password = TbPasswordNew.Text.ToString();
        comp.emergencyNum = TbEmergencyNum.Text.ToString();
        return comp;
    }

    //Method Creates a new Company with the passed in values
    private void CreateNewCompany(Token auth, Company comp)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/addCompany", Method.POST);
        request.AddParameter("email", comp.email); //email fro Textbox
        request.AddParameter("password", comp.password); //Pwd from Textbox
        request.AddParameter("address", comp.address); //address from Textbox
        request.AddParameter("emergencyNum", comp.emergencyNum); //emergencyNum from Textbox
        request.AddParameter("phone", comp.phone); //phone from Textbox
        request.AddParameter("name", comp.name); //name from Textbox

        request.AddHeader("Authorization", auth.token_type + " " + auth.access_token);

        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<BizCred>(response.Content);
        var bizObj = jsonObject as BizCred;

        //If Success
        if (bizObj.success == true)
        {
            TbEmail.Text = comp.email;
            //Trigger the JS 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('toggle');</script>", false);
        }
        //If Failed To Create A new Buisiness
        else
            DivFailedNewComp.Visible = true;
    }
}