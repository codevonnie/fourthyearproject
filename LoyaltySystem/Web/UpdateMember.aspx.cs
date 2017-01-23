using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Customer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DragAndDrop_Click(object sender, EventArgs e)
    {
        var dfhfghgf = "";
        //fileSelected.InnerText = "DROP MASTER";
    }















    string port = "http://localhost:8100/";


    /*Method Creates a new Customer Obj from the input form
     */
    //private Customer createCustomer()
    //{
    //    Customer customer = new Customer();
    //    customer.name = TbName.Text.ToString();
    //    customer.address = TbAddress.Text.ToString();
    //    customer.dob = Convert.ToDateTime(TbDob.Text);
    //    customer.gender = TbGender.Text.ToString();
    //    customer.contactNumber = Convert.ToInt32(TbContactNum.Text);
    //    customer.emergencyNumber = Convert.ToInt32(TbEmergencyNum.Text);
    //    customer.emergencyName = TbEmergencyName.Text;
    //    customer.email = TbEmail.Text.ToString();
    //    customer.date = DateTime.Now;//Todays Date

    //    customer.guardianName = TbGuardianName.Text.ToString();
    //    customer.guardianNumber = TbGuardianNumber.Text.ToString();

    //    customer.customerToken = Session["authToken"].ToString();//Store the authToken in customer.customerToken

    //    return customer;
    //}

    private void newCustomerRequest(Customer customer)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/addperson", Method.POST);
        request.AddParameter("x-access-token", customer.customerToken);
        request.AddParameter("name", customer.name);
        request.AddParameter("password", "pass");//random password
        request.AddParameter("email", customer.email);
        request.AddParameter("phone", customer.contactNumber);
        request.AddParameter("joined", customer.date);
        request.AddParameter("address", customer.address);
        request.AddParameter("dob", customer.dob);
        request.AddParameter("icename", customer.iceName);
        request.AddParameter("icephone", customer.icePhone);



        /* if (customer.guardianName !="" && customer.guardianNumber!="") {
             request.AddParameter("guardianName", customer.guardianName); 
             request.AddParameter("guardianNum", customer.guardianNumber); 
         }*/

        IRestResponse response = client.Execute(request);
        var content = response.Content;


        dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        var bizObj = jsonObject as BuisinessRoot;

        //If the Message is not Empty
        if (bizObj.message.Count != 0)
        {
            //Successful Login
            Server.Transfer("Default.aspx", true);
            newRelationshipRequest(customer, bizObj);
        }
        else
        {

        }

    }

    private void newRelationshipRequest(Customer customer, BuisinessRoot biz)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/addrelationship", Method.POST);
        request.AddParameter("name", customer.name);
        request.AddParameter("name", biz.message[0].name);//HARD CODDED FOR NOW FROM WHO EVER IS LOGGED IN

        //CACHE THE BUISNESS NAME TO SEND WITH THE CREATE RELATIONSHIP REQUEST

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string
    }



    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        //Customer cus = createCustomer();//CREATE A NEW CUSTOMER
        //newCustomerRequest(cus);//ADD THE NEW CUSTOMER TO THE DATABASE
    }
}