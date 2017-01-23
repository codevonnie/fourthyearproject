using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_SignInPage : System.Web.UI.Page
{

    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    private string _auth_Token = "";
    private string _auth_Type = "";

    public class Response
    {
        public bool success { get; set; }
        public string message { get; set; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Page.SetFocus(TbQRCode);
    }

    private void GetUsrSettings()
    {
        //-------------------------------- CACHE AUTH Decrypted --------------------------------
        //Cache might be cleared so need to get another token
        _auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        _auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());
    }


    protected void BtnCheckMember_Click(object sender, EventArgs e)
    {
        //var custObj = ParseQRCode();
        //FindPerson(custObj);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);



        //WORKS 
       // string jquery = "openModal();";
       // ClientScript.RegisterStartupScript(typeof(Page), "a key", "<script type=\"text/javascript\">" + jquery + "</script>");

        //  <button type="button" id="BtnCheckMember" class="btn btn-primary btn-block btn-lg" runat="server" data-toggle="modal" data-target="#myModal">Check Member</button>

    }

    private Customer ParseQRCode()
    {
        var custObj = new Customer();
        try
        {
            dynamic custObject = JsonConvert.DeserializeObject<Customer>(TbQRCode.Text);
            custObj = custObject as Customer;
        }
        catch (Exception)
        {
            // Catch unknown formats eg wrong QRcode or person id
        }
        return custObj;
    }



    //Send request to api and find the person
    private void FindPerson(Customer cust)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/findPerson", Method.POST);
        request.AddParameter("name", cust.name);
        request.AddParameter("address", cust.address);
        request.AddParameter("dob", cust.dob);
        request.AddParameter("phone", cust.phone);
        request.AddParameter("iceName", cust.iceName);
        request.AddParameter("icePhone", cust.icePhone);
        request.AddParameter("joined", cust.joined);//milliseconds ?
        request.AddParameter("email", cust.email);
        request.AddParameter("imgUrl", cust.imgUrl);
        request.AddParameter("guardianName", cust.guardianName);
        request.AddParameter("guardianNum", cust.guardianNum);
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);

        //"success": true,
        //      "name": "Paul Potts4564",
        //      "dob": "2315648943215",
        //      "address": "123 Fake Street5464",
        //      "phone": "353879876543",
        //      "iceName": "Bob Potts5464",
        //      "icePhone": "353871234567",
        //      "joined": "1484921393189",
        //      "email": "test@email.com",
        //      "imgUrl": "https://res.cloudinary.com/hlqysoka2/image/upload/v1484837295/itxmpiumdiu56q7sbebn.jpg",
        //      "guardianName": "timtim",
        //      "guardianNum": "1800696969"




        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<Response>(response.Content);
        var resObj = jsonObject as Response;

        if (resObj.success == true)
        {
            //Person was found Display to the Company
        }

    }




}