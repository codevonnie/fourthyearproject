using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for Authorization
/// </summary>
public class Authorization
{
    private static string clientId = WebConfigurationManager.AppSettings["AUTH0_CID"];
    private static string clientSecret = WebConfigurationManager.AppSettings["AUTH0_CS"];
    private static string apiEndPort = WebConfigurationManager.AppSettings["API_END_PORT"];
    private static string auth0EndPort = WebConfigurationManager.AppSettings["AUTH0_PORT"];

    public static Token GetAuth()
    {
        var client = new RestClient(auth0EndPort);
        var request = new RestRequest(Method.POST);
        request.AddHeader("content-type", "application/json");
        request.AddParameter("application/json", "{\"client_id\":\""+clientId+"\",\"client_secret\":\""+clientSecret+"\",\"audience\":\""+ apiEndPort + "/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);

        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<Token>(response.Content);
        var bizObj = jsonObject as Token;// ACCESS TOKEN USED TO GET AUTHENTICATED 

        // ------------------------ SET COOKIE NOT WORKING 100% ------------------------  
        #region
        //System.Web.HttpCookie newCookie = new System.Web.HttpCookie("AuthCookie");
        //newCookie.Values.Add("AC_T", bizObj.access_token);
        //newCookie.Values.Add("TYPE", bizObj.token_type);
        //newCookie.Secure = true;
        //newCookie.Name = "AuthCookie";
        //newCookie.Expires = DateTime.Now.AddMinutes(10.0);
        //newCookie.Path = "/";//-------------

        //Response.Cookies.Add(newCookie);
        #endregion

        return bizObj;
    }

}