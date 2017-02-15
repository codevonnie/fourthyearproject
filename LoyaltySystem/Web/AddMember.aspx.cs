using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;

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
        if (Page.IsValid)
        {
            CloudinaryApi.results cloudImg = new CloudinaryApi.results();
            try
            {
                cloudImg = StoreImgOnCloudinary();

                newCustomerRequest(cloudImg);
            }
            catch (Exception)
            {
                DivFailed.Visible = true;
                DeleteImage(cloudImg);
            }
        }
    }

    //---------------- Create/Return Customer Object ----------------
    private TempCustomer createCustomer()
    {
        TempCustomer customer = new TempCustomer();
        customer.name = TbName.Text.ToString();
        customer.address = TbAddress.Text.ToString();
        customer.dob = TbDob.Text;
        customer.phone = TbContactNum.Text.ToString();
        customer.icePhone = TbEmergencyNum.Text;
        customer.iceName = TbEmergencyName.Text;
        customer.email = TbEmail.Text.ToString();

        customer.guardianName = TbGuardianName.Text.ToString();
        customer.guardianNum = TbGuardianNumber.Text.ToString();
        return customer;
    }


    //---------------- Post to API Route addPerson  ---------------- ***************** FIX *********
    //NEEDS VALIDATION ON INPUTS
    private void newCustomerRequest(CloudinaryApi.results imgDetails)
    {
        TempCustomer cust = createCustomer();
        ConvertToMillSec convert = new ConvertToMillSec();

        var client = new RestClient(port);

        //Generate a random Password and replaces all non alphanumeric wiht numbers
        string password = Membership.GeneratePassword(6, 3);
        password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");

        var request = new RestRequest("addPerson", Method.POST);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
        request.AddParameter("name", cust.name);
        request.AddParameter("password", "*x*" + password);//random password
        request.AddParameter("email", cust.email);
        request.AddParameter("phone", cust.phone);
        request.AddParameter("address", cust.address);
        request.AddParameter("iceName", cust.iceName);
        request.AddParameter("icePhone", cust.icePhone);
        request.AddParameter("imgUrl", imgDetails.secure_url);
        request.AddParameter("publicImgId", imgDetails.public_id);

        request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

        var dobMill = convert.DateToMillSec(Convert.ToDateTime(cust.dob));
        request.AddParameter("dob", dobMill.ToString());

        //ONLY IF UNDER 18
        if (cust.guardianName != "" && cust.guardianNum != "")
        {
            request.AddParameter("guardianName", cust.guardianName);
            request.AddParameter("guardianNum", cust.guardianNum);
        }

        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {
            DivSuccess.Visible = true;
            ClearAddForm();
        }
    }


    //Deletes The Image From The Database
    private void DeleteImage(CloudinaryApi.results imgDetails)
    {
        string key = WebConfigurationManager.AppSettings["CLOUDINARY_API_KEY"];
        string secret = WebConfigurationManager.AppSettings["CLOUDINARY_API_SECRET"];
        string name = WebConfigurationManager.AppSettings["CLOUDINARY_API_NAME"];


        Account account = new Account(name, key, secret);
        Cloudinary cloudinary = new Cloudinary(account);


        var delParams = new DelResParams()
        {
            PublicIds = new List<string>() { imgDetails.public_id },
            Invalidate = true
        };
        var delResult = cloudinary.DeleteResources(delParams);
    }




    //Clear The Form
    private void ClearAddForm()
    {
        TbAddress.Text = "";
        TbContactNum.Text = "";
        TbDob.Text = "";
        TbEmail.Text = "";
        TbEmergencyName.Text = "";
        TbGuardianName.Text = "";
        TbAddress.Text = "";
        TbEmergencyNum.Text = "";
        TbName.Text = "";
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


            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(Server.MapPath("/App_Data/Images/" + file.FileName)),
                Transformation = new Transformation().Width(300).Height(400).Crop("limit"),
                Invalidate = true
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
