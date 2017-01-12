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

public partial class _Default : System.Web.UI.Page
{
    private string port = "http://localhost:8100/";
    private System.Web.HttpCookie authCookie;

    private void Page_Load(object sender, EventArgs e)
    {
        GetCookies();

        GetResponse();
    }

    private void GetCookies()
    {
        try
        {
            authCookie = Request.Cookies["AuthCookie"];
        }
        catch (Exception)
        {
            //Get a new Token? or ask use to Login again
            Server.Transfer("LoginPage.aspx", true);
            //Must be server.transfer otherwise Cookies wont work
        }
    }

    protected void AddCustomerBtn_Click(object sender, EventArgs e)
    {
        Server.Transfer("AddCustomer.aspx", true);
        System.Web.HttpCookie authCookie = Request.Cookies["AuthCookie"];
    }

    //Surround in a try catch
    private void GetResponse()
    {
        var client = new RestClient(port);

        string auth_token = authCookie.Values["AC_T"];
        string authType = authCookie.Values["TYPE"];


        var request = new RestRequest("api/businessMembers", Method.GET);  //----------------------Get Customers not Business
        request.AddHeader("Authorization", authType + " " + auth_token);

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