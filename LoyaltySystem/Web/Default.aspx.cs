using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using System.Diagnostics;
using System.Web.Security;
using System.Security.Principal;

public partial class _Default : System.Web.UI.Page
{
    private string port = "http://localhost:8100/";
    private System.Web.HttpCookie authCookie;
    private string _auth_Token = "";
    private string _auth_Type = "";

    private void Page_Load(object sender, EventArgs e)
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

            GetCustomers();
        }
    }

    private void GetUsrSettings()
    {
        //-------------------------------- CACHE AUTH Decrypted --------------------------------
        //Cache might be cleared so need to get another token
        _auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        _auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());

        //-------------------------------- DECRYPTION COOKIES --------------------------------
        #region
        //System.Web.HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //if (authCookie != null)
        //{
        //    //Extract the forms authentication cookie
        //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        //    // If caching roles in userData field then extract
        //    string[] roles = authTicket.UserData.Split(new char[] { '|' });

        //    // Create the IIdentity instance
        //    IIdentity id = new FormsIdentity(authTicket);

        //    // Create the IPrinciple instance
        //    IPrincipal principal = new GenericPrincipal(id, roles);

        //    // Set the context user 
        //    Context.User = principal;
        //}
        #endregion

        //-------------------------------- SSL COOKIES --------------------------------
        #region
        //try
        //{
        //    //SSL Cookie with Auth Token etc
        //    _bizName = Request.Cookies["BIZ_DETAILS"]["BIZ_NAME"];
        //    _authtoken = Request.Cookies["AuthCookie"]["AC_T"];
        //    _authType = Request.Cookies["AuthCookie"]["TYPE"];
        //}
        //catch (Exception)
        //{
        //    //Get a new Token? or ask use to Login again
        //    Response.Redirect("LoginPage.aspx", true);
        //}
        #endregion

    }

    //Surround in a try catch
    private void GetCustomers()
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/businessMembers", Method.GET);  //----------------------Get Customers not Business
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        var bizObj = jsonObject as BuisinessRoot;

        bindData(bizObj);
    }


    private void bindData(BuisinessRoot bizObj)
    {
        GridView1.DataSource = bizObj.message;//binds the Grid to the OBJECTS in the list
        GridView1.DataBind();
    }
}