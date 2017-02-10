using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;


public partial class AddMember : System.Web.UI.Page
{

    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //private string port = WebConfigurationManager.AppSettings["API_PORT"];

    //SSL Cookie with Auth Token etc
    private UserSettings settings = new UserSettings();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            settings = Cache.Get("Settings") as UserSettings;
            if (settings._loggedIn == null || settings._biz_Email == null || settings._auth_Token == null || settings._auth_Type == null)
                return;

            init();
        }
        catch (Exception)
        {
            Response.Redirect("LoginPage.aspx", true);
        };

    }

    private void init()
    {
        DivSuccess.Visible = false;
        DivFailed.Visible = false;
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            CloudinaryApi.results cloudImg = StoreImgOnCloudinary();

            newCustomerRequest(cloudImg);
        }
        catch (Exception)
        {
            DivFailed.Visible = true;
        }
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

        //if (TbMember.Text != "")
        //    customer.membership = Convert.ToDateTime(TbMember.Text);//Todays Date



        customer.guardianName = TbGuardianName.Text.ToString();
        customer.guardianNum = TbGuardianNumber.Text.ToString();
        return customer;
    }


    //---------------- Post to API Route addPerson  ---------------- ***************** FIX *********
    //NEEDS VALIDATION ON INPUTS
    private void newCustomerRequest(CloudinaryApi.results imgDetails)
    {
        Customer customer = createCustomer();
        ConvertToMillSec convert = new ConvertToMillSec();

        var client = new RestClient(port);
        string password = Membership.GeneratePassword(6, 3);

        var request = new RestRequest("addPerson", Method.POST);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
        request.AddParameter("name", customer.name);
        request.AddParameter("password", "*x*" + password);//random password
        request.AddParameter("email", customer.email);
        request.AddParameter("phone", customer.contactNumber);
        request.AddParameter("address", customer.address);
        request.AddParameter("iceName", customer.iceName);
        request.AddParameter("icePhone", customer.icePhone);
        request.AddParameter("imgUrl", imgDetails.secure_url);

        request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

        var dobMill = convert.DateToMillSec(customer.dob);
        request.AddParameter("dob", dobMill);

        //ONLY IF UNDER 18
        if (customer.guardianName != "" && customer.guardianNum != "")
        {
            request.AddParameter("guardianName", customer.guardianName);
            request.AddParameter("guardianNum", customer.guardianNum);
        }

        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {
            DivSuccess.Visible = true;
        }
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