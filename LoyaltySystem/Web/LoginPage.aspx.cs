using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginPage : System.Web.UI.Page
{
    string port = "http://localhost:8100/";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //stores the token and token type res from AUTH0
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }

    //stores the successfull logged in user
    public class PersonCredObject
    {
        public bool success { get; set; }
        public List<object> message { get; set; }
    }



    private void GetToken()
    {
        var client = new RestClient("https://membermeauth.eu.auth0.com/oauth/token");
        var request = new RestRequest(Method.POST);
        request.AddHeader("content-type", "application/json");
        request.AddParameter("application/json", "{\"client_id\":\"fXqMFIGFPGXAPLNm6ltd0NsGV6fWpvDM\",\"client_secret\":\"HHnBRmKTpK99fx4RYIVnxiJFQourT1RkbWnrs0jIUP1vdYrgWZ1104Tew7cb5-wp\",\"audience\":\"https://restapicust.herokuapp.com/api/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);

        var content = response.Content; // raw content as string

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<Token>(response.Content);
        var bizObj = jsonObject as Token;// ACCESS TOKEN USED TO GET AUTHENTICATED 

        checkLoginDetails(bizObj);
    }


    private void checkLoginDetails(Token auth)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/authenticate", Method.POST);
        request.AddParameter("email", TbEmail.Text); //email fro Textbox
        request.AddParameter("password", TbPassword.Text); //pwd from Textbox
        request.AddHeader("Authorization", auth.token_type + " " + auth.access_token);

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<PersonCredObject>(response.Content);
        var bizObj = jsonObject as Token;

        // Session["authToken"] = bizObj.access_token;// store the token in a session

        //If the Message is Empty
        if (bizObj.access_token != "")
        {
            //Successful Login
            Server.Transfer("Default.aspx", true);
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