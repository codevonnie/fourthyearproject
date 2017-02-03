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


    //=================================================================> Need To Setup Post Route / Methods. <=================================================================


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                GetUsrSettings();
            }
            catch (Exception)
            {
                Response.Redirect("LoginPage.aspx", true);
            };
        }
    }

    private void GetUsrSettings()
    {
        //-------------------------------- CACHE AUTH--------------------------------
        settings._auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        settings._auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());
        settings._loggedIn = Decrypt.Base64Decode(Cache.Get("Auth_LoggedIn").ToString());
    }


    protected void BtnDeleteMember_Click(object sender, EventArgs e)
    {
        //var client = new RestClient(port);
        //var request = new RestRequest("api/addPerson", Method.POST);
        //request.AddHeader("Authorization", settings._auth_Type + " " + settings._auth_Token);
        //request.AddParameter("email", TbEmail.Text);

        //IRestResponse response = client.Execute(request);
        //var content = response.Content;

        //if (content!="")
        //{

        //}
        //else
        //{
        //    Type cstype = this.GetType();

        //    // Get a ClientScriptManager reference from the Page class.
        //    ClientScriptManager cs = Page.ClientScript;

        //    // Check to see if the startup script is already registered.
        //    if (!cs.IsStartupScriptRegistered(cstype, "PopupScript"))
        //    {
        //        String cstext = "alert('');";
        //        cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
        //    }

        //}
    }
}