using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Customer : System.Web.UI.Page
{

    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //  private string port = WebConfigurationManager.AppSettings["API_PORT"];
    // private System.Web.HttpCookie authCookie;

    //SSL Cookie with Auth Token etc
    private object _auth_Token = "";
    private object _auth_Type = "";
    private object _biz_Name = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            try
            {
                GetUsrSettings();
            }
            catch (Exception)
            {
                Response.Redirect("LoginPage.aspx", true);
            };
        }
    }

    private void GetUsrSettings()
    {
        //-------------------------------- CACHE AUTH--------------------------------
        //Cache might be cleared so need to get another token
        _auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        _auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());
        _biz_Name = Cache.Get("BizName");

        //-------------------------------- DECRYPTION COOKIES --------------------------------
        #region
        //System.Web.HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //if (authCookie != null)
        //{
        //    //Extract the forms authentication cookie
        //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        //    // If caching roles in userData field then extract
        //    string[] roles = authTicket.UserData.Split(new char[] { '|' });

        //    // Create the IIdentity instance
        //    IIdentity id = new FormsIdentity(authTicket);

        //    // Create the IPrinciple instance
        //    IPrincipal principal = new GenericPrincipal(id, roles);

        //    // Set the context user 
        //    Context.User = principal;
        //}
        #endregion

        //-------------------------------- SSL COOKIES --------------------------------
        #region
        //try
        //{
        //    //SSL Cookie with Auth Token etc
        //    _bizName = Request.Cookies["BIZ_DETAILS"]["BIZ_NAME"];
        //    _authtoken = Request.Cookies["AuthCookie"]["AC_T"];
        //    _authType = Request.Cookies["AuthCookie"]["TYPE"];
        //}
        //catch (Exception)
        //{
        //    //Get a new Token? or ask use to Login again
        //    Response.Redirect("LoginPage.aspx", true);
        //}
        #endregion

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
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);
        request.AddParameter("name", customer.name);
        request.AddParameter("password", "¬¬¬" + password);//random password
        request.AddParameter("email", customer.email);
        request.AddParameter("phone", customer.contactNumber);
        request.AddParameter("gender", customer.gender);
        request.AddParameter("joined", customer.date.ToString("MMMM dd, yyyy"));
        request.AddParameter("address", customer.address);
        request.AddParameter("dob", customer.dob.ToString("MMMM dd, yyyy"));
        request.AddParameter("icename", customer.emergencyName);
        request.AddParameter("icephone", customer.emergencyNumber);


        //ONLY IF UNDER 18
        if (customer.guardianName != "" && customer.guardianNumber != "")
        {
            request.AddParameter("guardianName", customer.guardianName);
            request.AddParameter("guardianNum", customer.guardianNumber);
        }

        IRestResponse response = client.Execute(request);
        var content = response.Content;


        //FIX RESPONSE JSON
        // dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        // var bizObj = jsonObject as BuisinessRoot;

        //If the Message is not Empty
        if (content != "")
        {
            //Successful Login
            Server.Transfer("Default.aspx", true);
            //newRelationshipRequest(customer, bizObj);
        }
        else
        {

        }

    }

    /*
     * Method Creates a new Relationship between the Biz and the newly added person
     */
    private void newRelationshipRequest(Customer customer, BuisinessRoot biz)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/addRelationship", Method.POST);
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);
        request.AddParameter("name", customer.name);
        request.AddParameter("name", _biz_Name);//lOGGED IN BIZ NAME

        IRestResponse response = client.Execute(request);
        var content = response.Content; // raw content as string
    }



    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        Customer cus = createCustomer();//CREATE A NEW CUSTOMER
        newCustomerRequest(cus);//ADD THE NEW CUSTOMER TO THE DATABASE
    }
}