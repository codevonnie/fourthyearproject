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

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtnDeleteMember_Click(object sender, EventArgs e)
    {
        var client = new RestClient(port);
        var request = new RestRequest("api/addperson", Method.POST);
        request.AddParameter("x-access-token", Session["authToken"].ToString());
        request.AddParameter("name", TbMember.Text);
        request.AddParameter("email", TbMember.Text);

        IRestResponse response = client.Execute(request);
        var content = response.Content;
        //Response Customer Deleted ?

    }
}