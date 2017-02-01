using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Web_Customer : System.Web.UI.Page
{

    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    // private string port = WebConfigurationManager.AppSettings["API_PORT"];

    //SSL Cookie with Auth Token etc
    private object _auth_Token = "";
    private object _auth_Type = "";
    private object _biz_Name = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetUsrSettings();
        }
        catch (Exception)
        {
            Response.Redirect("LoginPage.aspx", true);
        };

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

    //---------------- Create/Return Customer Object ----------------
    private Customer createCustomer()
    {
        Customer customer = new Customer();
        customer.name = TbName.Text.ToString();
        customer.address = TbAddress.Text.ToString();
        customer.dob = Convert.ToDateTime(TbDob.Text);
        customer.contactNumber = TbContactNum.Text;
        customer.icePhone = TbEmergencyNum.Text;
        customer.iceName = TbEmergencyName.Text;
        customer.email = TbEmail.Text.ToString();
        customer.date = DateTime.Now;//Todays Date

        customer.guardianName = TbGuardianName.Text.ToString();
        customer.guardianNum = TbGuardianNumber.Text.ToString();
        return customer;
    }


    //---------------- Post to API Route addPerson  ----------------
    private void newCustomerRequest(CloudinaryApi.results imgDetails)
    {
        Customer customer = createCustomer();

        var client = new RestClient(port);
        string password = Membership.GeneratePassword(6, 3);

        var request = new RestRequest("api/addPerson", Method.POST);
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);
        request.AddParameter("name", customer.name);
        request.AddParameter("password", "*x*" + password);//random password
        request.AddParameter("email", customer.email);
        request.AddParameter("phone", customer.contactNumber);
        request.AddParameter("joined", customer.date.ToString("MMMM dd, yyyy"));
        request.AddParameter("address", customer.address);      
        request.AddParameter("icename", customer.iceName);
        request.AddParameter("icephone", customer.icePhone);
        request.AddParameter("imgUrl", imgDetails.secure_url);

        //http://stackoverflow.com/questions/5955883/datetimes-representation-in-milliseconds
        //Needed to get milliseconds for database
        string simpleDate = customer.dob.ToString("dd/MM/yyyy");
        DateTime dt = DateTime.ParseExact(simpleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        var mil = dt.ToUniversalTime().Subtract(
      new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      ).TotalMilliseconds;

        request.AddParameter("dob", mil);


        //ONLY IF UNDER 18
        if (customer.guardianName != "" && customer.guardianNum != "")
        {
            request.AddParameter("guardianName", customer.guardianName);
            request.AddParameter("guardianNum", customer.guardianNum);
        }

        IRestResponse response = client.Execute(request);
        var content = response.Content;

        //FIX RESPONSE JSON
        //dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        //var bizObj = jsonObject as BuisinessRoot;

        //If the Message is not Empty
        if (content != "")
            Server.Transfer("Default.aspx", true);
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
        CloudinaryApi.results cloudImg = StoreImgOnCloudinary();

        newCustomerRequest(cloudImg);//ADD THE NEW CUSTOMER TO THE DATABASE
    }


    // --------------------------- Upload image To The Database On Cloudinary --------------------------- 
    private CloudinaryApi.results StoreImgOnCloudinary()
    {
        string key = WebConfigurationManager.AppSettings["CLOUDINARY_API_KEY"];
        string secret = WebConfigurationManager.AppSettings["CLOUDINARY_API_SECRET"];
        string name = WebConfigurationManager.AppSettings["CLOUDINARY_API_NAME"];

        dynamic jsonObject = new CloudinaryApi.results();

        CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(name, key, secret);
        CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);


        HttpPostedFile file = Request.Files["ctl00$ContentPlaceHolder1$ImagePath"];

        //Check To See If File has contents
        //------------------- error check file type ? ------------------------------
        if (file != null && file.ContentLength > 0)
        {
            string fname = Path.GetFileName(file.FileName);
            file.SaveAs(Server.MapPath(Path.Combine("~/App_Data/Images", fname)));

            #region
            // input = new byte[file.ContentLength];
            //System.IO.Stream MyStream;
            //// Initialize the stream.
            //MyStream = file.InputStream;

            //// Read the file into the byte array.
            //MyStream.Read(input, 0, file.ContentLength);

            //pathImg = "data:image/"
            //                + Path.GetExtension(file.FileName).Replace(".", "")
            //                + ";base64,"
            //                + Convert.ToBase64String(input) + "\" />";
            #endregion

            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(Server.MapPath("/App_Data/Images/" + file.FileName))
            };

            //Upload Image
            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

            jsonObject = JsonConvert.DeserializeObject<CloudinaryApi.results>(uploadResult.JsonObj.ToString());

            //Remove the Image from storage again
            File.Delete(Server.MapPath(Path.Combine("~/App_Data/Images", fname)));
        }
        return (jsonObject as CloudinaryApi.results);
    }
}