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
using System.Web.Configuration;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //private string port = WebConfigurationManager.AppSettings["API_PORT"];
    // private System.Web.HttpCookie authCookie;

    private UserSettings settings = new UserSettings();
    private static List<TempCustomer> _tempCust;

    private void Page_Load(object sender, EventArgs e)
    {

        try
        {
            settings = Cache.Get("Settings") as UserSettings;
            if (settings._loggedIn == null || settings._biz_Email == null || settings._auth_Token == null || settings._auth_Type == null)
                return;
        }
        catch (Exception)
        {
            Response.Redirect("LoginPage.aspx", true);
        };
    }



    //Surround in a try catch
    private void GetCustomers()
    {
        //var client = new RestClient(port);

        //var request = new RestRequest("businessMembers", Method.GET);  //----------------------Get Customers not Business
        //request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));

        //IRestResponse response = client.Execute(request);
        //var content = response.Content; // raw content as string

        ////Deserialize the result into the class provided
        //dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        //var bizObj = jsonObject as BuisinessRoot;

        //bindData(bizObj);
    }



    private void bindData(BuisinessRoot bizObj)
    {
        GridView1.DataSource = bizObj.message;//binds the Grid to the OBJECTS in the list
        GridView1.DataBind();
    }

}