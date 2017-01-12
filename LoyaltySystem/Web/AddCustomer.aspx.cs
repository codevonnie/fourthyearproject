using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Customer : System.Web.UI.Page
{

    private string port = "http://localhost:8100/";
    // private System.Web.HttpCookie authCookie;

    //SSL Cookie with Auth Token etc
    private string _authtoken = "";
    private string _authType = "";
    private string _bizName = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        GetCookies();
    }

    private void GetCookies()
    {
        try
        {
            //SSL Cookie with Auth Token etc
            _bizName = Request.Cookies["BIZ_DETAILS"]["BIZ_NAME"];
            _authtoken = Request.Cookies["AuthCookie"]["AC_T"];
            _authType = Request.Cookies["AuthCookie"]["TYPE"];
        }
        catch (Exception)
        {
            //Get a new Token? or ask use to Login again
            Response.Redirect("LoginPage.aspx", true);
        }
    }

    /*Method Creates a new Customer Obj from the input form
     */
    private Customer createCustomer()
    {
        Customer customer = new Customer();
        customer.name = TbName.Text.ToString();
        customer.address = TbAddress.Text.ToString();
        customer.dob = Convert.ToDateTime(TbDob.Text);
        customer.gender = TbGender.Text.ToString();
        customer.contactNumber = Convert.ToInt32(TbContactNum.Text);
        customer.emergencyNumber = Convert.ToInt32(TbEmergencyNum.Text);
        customer.emergencyName = TbEmergencyName.Text;
        customer.email = TbEmail.Text.ToString();
        customer.date = DateTime.Now;//Todays Date

        customer.guardianName = TbGuardianName.Text.ToString();
        customer.guardianNumber = TbGuardianNumber.Text.ToString();
        return customer;
    }

    private void newCustomerRequest(Customer customer)
    {
        var client = new RestClient(port);
        string password = Membership.GeneratePassword(6, 3);

        var request = new RestRequest("api/addPerson", Method.POST);
        request.AddHeader("Authorization", _authType + " " + _authtoken);
        request.AddParameter("name", customer.name);
        request.AddParameter("password", "¬¬¬" + password);//random password
        request.AddParameter("email", customer.email);
        request.AddParameter("phone", customer.contactNumber);
        request.AddParameter("gender", customer.gender);
        request.AddParameter("joined", customer.date);
        request.AddParameter("address", customer.address);
        request.AddParameter("dob", customer.dob);
        request.AddParameter("icename", customer.emergencyName);
        request.AddParameter("icephone", customer.emergencyNumber);


        //ONLY IF UNDER 18
        //if (customer.guardianName != "" && customer.guardianNumber != "")
        //{
        //    request.AddParameter("guardianName", customer.guardianName);
        //    request.AddParameter("guardianNum", customer.guardianNumber);
        //}

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
        request.AddHeader("Authorization", _authType + " " + _authtoken);
        request.AddParameter("name", customer.name);
        request.AddParameter("name", _bizName);//lOGGED IN BIZ NAME

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string
    }



    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        Customer cus = createCustomer();//CREATE A NEW CUSTOMER
        newCustomerRequest(cus);//ADD THE NEW CUSTOMER TO THE DATABASE
    }
}