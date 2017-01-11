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

    private class Token
    {
        public string success { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }

    private void UserLogin()
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/authenticate", Method.POST);
        request.AddParameter("email", TbEmail.Text); //email fro Textbox
        request.AddParameter("password", TbPassword.Text); //pwd from Textbox

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<Token>(response.Content);
        var bizObj = jsonObject as Token;

        Session["authToken"] = bizObj.token;// store the token in a session

        //If the Message is Empty
        if (bizObj.token != "")
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
        UserLogin();
    }
}