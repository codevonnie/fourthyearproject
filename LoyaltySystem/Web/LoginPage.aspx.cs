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

    }

    //stores the successfull logged in user
    public class BizCred
    {
        public bool success { get; set; }
        public string name { get; set; }
    }


    //METHOD MUST HAVE TRY CATCH AND DISPLAY TO USER AND ERROR
    private void checkLoginDetails(Token auth)
    {
        var client = new RestClient(port);

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

            // ------------------------ TEMP CACHE KEYS ETC ------------------------ 
            Cache["BizName"] = Encrypt.Base64Encode(bizObj.name);

            Cache["Auth_LoggedIn"] = Encrypt.Base64Encode(successAuth);

            // ------------------------ TEMP CACHE KEYS ETC Encrypted ------------------------ 
            Cache["AuthToken"] = Encrypt.Base64Encode(auth.access_token);
            Cache["AuthType"] = Encrypt.Base64Encode(auth.token_type);

            //Successful Login
            Response.Redirect("AddMember.aspx", true);
        }
        else
        {
            //Nothing returned means Request Failed
        }
    }

    /* Button click Event from singInBtn
     */
    protected void singInBtn_Click(object sender, EventArgs e)
    {
        //Get a new token from AUTH0 and pass it in as a parameter to checkLoginDetails
        checkLoginDetails(Authorization.GetAuth());
    }
}