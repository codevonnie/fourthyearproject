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
    private Boolean isLogedIn = false;

    protected void Page_Load(object sender, EventArgs e)
    {
 
    }


    private void UserLogin()
    {
        var client = new RestClient("http://localhost:8080/");

        var request = new RestRequest("api/login", Method.POST);
        request.AddParameter("email", TbEmail.Text); //email fro Textbox
        request.AddParameter("password", TbPassword.Text); //pwd from Textbox

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string


        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        var bizObj = jsonObject as BuisinessRoot;

        //If the Message is Empty
        if (bizObj.message.Count != 0)
        {
            isLogedIn = true;
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