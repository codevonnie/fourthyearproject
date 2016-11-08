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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        GetResponse();
    }

    //Surround in a try catch
    private void GetResponse()
    {

        var client = new RestClient("https://restapicust.herokuapp.com");

        var request = new RestRequest("api/businessMembers", Method.GET);  //----------------------Get Customers not Business
        request.AddParameter("x-access-token", Session["authToken"].ToString());
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