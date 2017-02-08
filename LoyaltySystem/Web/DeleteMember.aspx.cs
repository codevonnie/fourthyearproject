using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_AddCustomer : System.Web.UI.Page
{
    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    // private string port = WebConfigurationManager.AppSettings["API_PORT"];

    //SSL Cookie with Auth Token etc
    private UserSettings settings = new UserSettings();

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
            try
            {
            settings = Cache.Get("Settings") as UserSettings;
            if (settings._loggedIn == null || settings._biz_Email == null || settings._auth_Token == null || settings._auth_Type == null)
                return;

            init();
            }
            catch (Exception)
            {
                Response.Redirect("LoginPage.aspx", true);
            };
       // }
    }

    private void init()
    {
        Page.SetFocus(TbEmail);//Refocus on InputBox

        //Err,Success Messages
        DivSuccess.Visible = false;
        DivFailed.Visible = false;
        DivConnectionErr.Visible = false;
    }


    protected void BtnDeleteMember_Click(object sender, EventArgs e)
    {
        var client = new RestClient(port);
        try
        {
            var request = new RestRequest("api/deletePerson", Method.DELETE);
            request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
            request.AddParameter("email", TbEmail.Text.ToString());
            request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

            IRestResponse response = client.Execute(request);

            //Deserialize the result into the class provided
            var jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
            ResponseMessage resObj = jsonObject as ResponseMessage;

            if (resObj.success == true)
                DivSuccess.Visible = true;
            else
                DivFailed.Visible = true;

        }
        catch (Exception)
        {
            DivConnectionErr.Visible = true;
        }

        TbEmail.Text = "";//Reset Text
    }
}