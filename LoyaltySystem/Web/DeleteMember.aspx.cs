using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_AddCustomer : System.Web.UI.Page
{
    string port = "http://localhost:8100/";

    //SSL Cookie with Auth Token etc
    private object _auth_Token = "";
    private object _auth_Type = "";
    private object _biz_Name = "";


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
        _auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        _auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());
        _biz_Name = Cache.Get("BizName");
    }


    protected void BtnDeleteMember_Click(object sender, EventArgs e)
    {
        var client = new RestClient(port);
        var request = new RestRequest("api/addPerson", Method.POST);
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);
        request.AddParameter("email", TbEmail.Text);

        IRestResponse response = client.Execute(request);
        var content = response.Content;

        if (content!="")
        {
           
        }
        else
        {
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(cstype, "PopupScript"))
            {
                String cstext = "alert('');";
                cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
            }

        }
    }
}