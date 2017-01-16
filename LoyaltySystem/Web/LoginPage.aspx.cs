using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginPage : System.Web.UI.Page
{
    string port = "http://localhost:8100/";
    // string port = "https://restapicust.herokuapp.com/";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //stores the successfull logged in user
    public class BizCred
    {
        public bool success { get; set; }
        public string name { get; set; }
    }



    private void GetToken()
    {
        var client = new RestClient("https://membermeauth.eu.auth0.com/oauth/token");
        var request = new RestRequest(Method.POST);
        request.AddHeader("content-type", "application/json");
        request.AddParameter("application/json", "{\"client_id\":\"fXqMFIGFPGXAPLNm6ltd0NsGV6fWpvDM\",\"client_secret\":\"HHnBRmKTpK99fx4RYIVnxiJFQourT1RkbWnrs0jIUP1vdYrgWZ1104Tew7cb5-wp\",\"audience\":\"https://restapicust.herokuapp.com/api/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<Token>(response.Content);
        var bizObj = jsonObject as Token;// ACCESS TOKEN USED TO GET AUTHENTICATED 

        System.Web.HttpCookie newCookie = new System.Web.HttpCookie("AuthCookie");
        newCookie.Values.Add("AC_T", bizObj.access_token);
        newCookie.Values.Add("TYPE", bizObj.token_type);
        newCookie.Secure = true;
        newCookie.Name = "AuthCookie";
        newCookie.Expires = DateTime.Now.AddMinutes(10.0);
        newCookie.Path = "~/";

        Response.Cookies.Add(newCookie);

        checkLoginDetails(bizObj);
    }


    private void checkLoginDetails(Token auth)
    {
        var client = new RestClient(port);
       // client.CookieContainer = new System.Net.CookieContainer();
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
            System.Web.HttpCookie newCookie = new System.Web.HttpCookie("BIZ_DETAILS");
            newCookie.Values.Add("BIZ_NAME", bizObj.name);
            newCookie.Values.Add("AUTH_SUCC", bizObj.success.ToString());
            newCookie.Secure = true;
            newCookie.Name = "BIZ_DETAILS";
            newCookie.Expires = DateTime.Now.AddMinutes(10.0);
            newCookie.Path = "~/";

            Response.Cookies.Add(newCookie);
            //Successful Login
            Response.Redirect("Default.aspx", true);
           // Server.Transfer("Default.aspx", true);
        }
        else
        {
            //Nothing returned means Request Failed
        }
    }

    /*Button click Event from singInBtn
     */
    protected void singInBtn_Click(object sender, EventArgs e)
    {
        GetToken();
    }
}