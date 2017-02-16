using Newtonsoft.Json;
using RestSharp;
using System;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;

using System.Linq;
using System.Web.Security;

using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Web.Script.Serialization;

public partial class Arivals : System.Web.UI.Page
{
    //private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    private string port = WebConfigurationManager.AppSettings["API_PORT"];

    private TempCustomer _TempCust = new TempCustomer();
    private Boolean _newCust;
    private string _custJson = "null";
    private ConvertToMillSec convert = new ConvertToMillSec();


    protected string GetCustomer { get { return _custJson; } }
    protected Boolean NewPerson { get { return _newCust; } }

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

    //-------------------------------- Init Method For Start Up -----------------------------------------------
    private void init()
    {
        Page.SetFocus(TbQRCode);//Refocus on InputBox


        //Update Control
        DivDisplay.Visible = false;
        MembershipControl.Visible = false;

        //Err,Success Messages
        DivSuccess.Visible = false;
        DivFailed.Visible = false;
        DivConnectionErr.Visible = false;
        DivSuccessCheckIn.Visible = false;
        DivDeletedMember.Visible = false;


        //PersonDetails
        ImgPerson.Visible = true;
        LblJoined.Visible = true;
        ImgPerson.Visible = true;
        PersonDetails.Visible = true;
        BtnCheckin.Visible = true;
        DropDownHide.Visible = true;
        DivJoined.Visible = true;



        DivTempPwd.Visible = false;
        DivFailedEmail.Visible = false;
        DivFailedScan.Visible = false;
    }



    /*=================================================================================> Find Person Methods <=====================================================================
    //****************************************************************************************************************************************************************************/


    //-------------------------------- Btn Check Member Click Event -------------------------------------------
    //Try Catch Calls Relevent Methods
    protected void BtnCheckMember_Click(object sender, EventArgs e)
    {
        //Check that something was entered into the text box first 
        try
        {
            String custEmail = TbQRCode.Text.ToString();
            FindPerson(custEmail);
        }
        catch (Exception)
        {
            DivFailedScan.Visible = true;
        }
    }


    //-------------------------------- Find Person -----------------------------------------------------
    //Send request to api and find the person
    private void FindPerson(string email)
    {
        var client = new RestClient(port);

        try
        {
            var request = new RestRequest("findPerson", Method.POST);
            request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
            request.AddParameter("email", email);
            request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

            IRestResponse response = client.Execute(request);

            var jsonObject = JsonConvert.DeserializeObject<TempCustomer>(response.Content);
            TempCustomer resObj = jsonObject as TempCustomer;

            if (resObj.success == true)
            {
                //Person was found Display to the Company               
                displayPerson(resObj);

                //Used To Trigger JS On Check in only
                Cache["CheckingIn"] = true;

                //Cached customer obj with timeout
                Cache.Insert("CUSTOMER_OBJ", resObj, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10)); //
            }
            else
                DivFailedScan.Visible = true;

        }
        catch (Exception)
        {
            DivConnectionErr.Visible = true;
        }

        TbQRCode.Text = "";//Reset Text
    }


    //-------------------------------- Display Person Modal ---------------------------------------------------
    //Displays the person on the screen once found
    private void displayPerson(TempCustomer cust)
    {
        var joined = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.joined.ToString()));

        LblName.Text = cust.name;
        LblJoined.Text = joined.ToString("dd/MM/yyyy");
        LblAge.Text = CalculateAge(cust.dob.ToString());
        LblEmail.Text = cust.email;

        //Dont Display Guardian Name/Num if Null
        if (cust.guardianName.ToString() == "null")
            GuardName.Visible = false;

        //Dont Display Guardian Name/Num if Null
        if (cust.guardianNum.ToString() == "null")
            GuardNum.Visible = false;

        //Dont Display Membership if Null
        if (cust.membership.ToString() != "0")
        {
            MembershipControl.Visible = true;
            var memberDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.membership.ToString()));
            LblMember.Text = memberDate.ToString("dd/MM/yyyy");
        }


        if (cust.datesVisited != null)
        {
            List<string> HsList = cust.datesVisited.ToList();
            var lastVisited = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(HsList.Max()));
            LblLastVisited.Text = lastVisited.ToString("dd/MM/yyyy");
        }



        LblGuardNum.Text = cust.guardianNum.ToString();
        LblGuardName.Text = cust.guardianName.ToString();

        LblIceName.Text = cust.iceName;
        LblIceNum.Text = cust.icePhone;
        LblTimesVisited.Text = cust.visited.ToString();

        ImgPerson.ImageUrl = cust.imgUrl;

        //Trigger the JS 
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }


    //-------------------------------- Calculate Age ----------------------------------------------------------
    //Caluclates the age of the Person
    private string CalculateAge(string dob)
    {
        //Might need validation *******
        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(dob));

        //Adapted From http://stackoverflow.com/questions/3054715/c-sharp-calculate-accurate-age
        DateTime today = DateTime.Today;

        int months = today.Month - date.Month;
        int years = today.Year - date.Year;

        if (today.Day < date.Day)
        {
            months--;
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        int days = (today - date.AddMonths((years * 12) + months)).Days;

        return string.Format("{0} Year{1}", years, (years == 1) ? "" : "s Old");

    }



    /*=================================================================================> Update Methods <==========================================================================
    //*****************************************************************************************************************************************************************************/


    //-------------------------------- Update Person Info, Check In + Update Btns -----------------------------------------------------------
    protected void UpdatePersonInfo_Click(object sender, EventArgs e)
    {
        try
        {
            UpdatePerson(sender); //check to see if text entered 
            TbUpdate.Text = "";//Reset The Text
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
            DivFailed.Visible = true;
        }
    }


    //-------------------------------- UpdatePerson -----------------------------------------------------------
    //Send request to api and find the person 
    private void UpdatePerson(object sender)
    {
        Button btn = sender as Button;
        TempCustomer cust = (TempCustomer)Cache.Get("CUSTOMER_OBJ");

        HashSet<string> visitHashSet = cust.datesVisited;

        //IF the List is empty create a new one
        if (visitHashSet == null)
            visitHashSet = new HashSet<string>();

        var client = new RestClient(port);

        //Check To see if the person is just checking in
        if (btn.ID == "BtnCheckin")
        {
            cust.tempEmail = cust.email;

            //Todays Current DateTim Now In Millseconds
            var today= convert.DateToMillSec(DateTime.Now).ToString();

            //Add the current Milliseconds to the list
            visitHashSet.Add(today);

            //Tempoary Data used for BarChart
            visitHashSet.Add("1487289600000");
            visitHashSet.Add("1487376000000");
            visitHashSet.Add("1487462400000");
            visitHashSet.Add("1489968000000");
            visitHashSet.Add("1503183600000");

            cust.datesVisited = visitHashSet;

            int num = int.Parse(cust.visited);
            num++;
            cust.visited = num.ToString();

        }
        else
        {
            if (btn.ID == "BtnRemoveGuard" || btn.ID == "BtnRemoveMembership")
                Cache["DD_CHOICE"] = btn.ID;

            cust = UpdateCustObj();
        }

        var request = new RestRequest("updatePerson", Method.PUT);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
        request.AddParameter("name", cust.name);
        request.AddParameter("email", cust.email);
        request.AddParameter("guardianName", cust.guardianName);
        request.AddParameter("guardianNum", cust.guardianNum);
        request.AddParameter("tempEmail", cust.tempEmail);
        request.AddParameter("visited", cust.visited);

        //Convert the cust.datesVisited to a json obj
        var jsonSerialiser = new JavaScriptSerializer();
        var datesVisitedJson = jsonSerialiser.Serialize(cust.datesVisited);

        request.AddParameter("datesVisited", datesVisitedJson);

        if (cust.membership != "0") // Could have a Membership Already so maby a function to check if membership is out-of-date
        {
            var memberDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.membership.ToString()));

            var mil = convert.DateToMillSec(memberDate);
            cust.membership = mil.ToString();//Update the Membership with a new Date (Miliseconds)
            MembershipControl.Visible = true;
        }

        request.AddParameter("membership", cust.membership);

        //SendRequest
        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {

            //Pass the Objects to Js here
            if ((Boolean)Cache.Get("CheckingIn") == true)
            {
                _newCust = true;
                dynamic jsonect = JsonConvert.SerializeObject(cust);
                _custJson = jsonect;
            }

            //Person UPDATED
            if (btn.ID == "BtnCheckin")
                DivSuccessCheckIn.Visible = true;
            else
                DivSuccess.Visible = true;

            LblTimesVisited.Text = cust.visited.ToString();
            cust.email = cust.tempEmail;
        }
        else
        {
            DivFailedEmail.Visible = true;
            LblEmail.Text = cust.email;
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }


    //Switch on the Person Property that they want to change
    private TempCustomer UpdateCustObj()
    {
        String choice = Cache.Get("DD_CHOICE").ToString();
        TempCustomer tempCust = (TempCustomer)Cache.Get("CUSTOMER_OBJ");

        tempCust.tempEmail = tempCust.email;

        switch (choice)
        {
            case "BtnUpName":
                tempCust.name = TbUpdate.Text.ToString();
                LblName.Text = TbUpdate.Text.ToString();
                break;

            case "BtnUpGuardName":
                tempCust.guardianName = TbUpdate.Text.ToString();
                LblGuardName.Text = TbUpdate.Text.ToString();
                GuardName.Visible = true;
                break;

            case "BtnUpGuardNum":
                tempCust.guardianNum = TbUpdate.Text.ToString();
                LblGuardNum.Text = TbUpdate.Text.ToString();
                GuardNum.Visible = true;
                break;

            case "BtnRemoveGuard":
                tempCust.guardianName = "null";
                tempCust.guardianNum = "null";

                LblGuardName.Text = tempCust.guardianName.ToString();
                LblGuardNum.Text = tempCust.guardianNum.ToString();
                GuardName.Visible = false;
                GuardNum.Visible = false;
                break;

            case "BtnRemoveMembership":
                tempCust.membership = "0";
                LblMember.Text = tempCust.membership.ToString();

                MembershipControl.Visible = false;
                break;

            case "BtnUpEmail":
                tempCust.tempEmail = TbUpdate.Text.ToString();
                LblEmail.Text = TbUpdate.Text.ToString();
                break;

            case "BthUpMembershipEndDate":
                ConvertToMillSec convert = new ConvertToMillSec();
                DateTime date = Convert.ToDateTime(TbUpdate.Text.ToString());
                var memMill = convert.DateToMillSec(date);

                tempCust.membership = memMill.ToString();
                MembershipControl.Visible = true;
                LblMember.Text = date.ToString("dd/MM/yyyy");
                break;
        }

        Cache.Insert("CUSTOMER_OBJ", tempCust, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10));

        return tempCust;
    }


    //-------------------------------- Update Password ---------------------------------------
    protected void BntResetPwd_Click(object sender, EventArgs e)
    {
        try
        {
            UpdatePwd();
        }
        catch (Exception)
        {
            DivConnectionErr.Visible = true;
        }
    }


    // Updates the users Pasword With a New Tempoary One
    private void UpdatePwd()
    {
        TempCustomer cust = (TempCustomer)Cache.Get("CUSTOMER_OBJ");

        //Generate a random Password and replaces all non alphanumeric wiht numbers
        string password = Membership.GeneratePassword(6, 3);
        password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");

        cust.tempPwd = "*x*" + password;

        var client = new RestClient(port);

        var request = new RestRequest("newPassword", Method.PUT);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
        request.AddParameter("email", cust.email);
        request.AddParameter("password", cust.tempPwd);

        //SendRequest
        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {
            DivTempPwd.InnerHtml = "Tempoary Password: <strong>" + cust.tempPwd + "</strong>";
            DivTempPwd.Visible = true;

        }
        else
            DivFailed.Visible = true;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }



    /*-------------------------------- Update Choice Click Event ---------------------------------------
    * Method Gets the users choice from the DropDown box when a person has Arrived, located in the "Bootstrap Modal"
    * Displays a "Update textbox" that updates its placeholder based on the Choice made.
    */
    protected void UpdateChoice_Click(object sender, EventArgs e)
    {
        Button btnInfo = sender as Button;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);

        TbUpdate.Focus();
        //If The Choice was "Membership End Date" turn the text mode to DATE
        if (btnInfo.ID == "BthUpMembershipEndDate")
            TbUpdate.TextMode = TextBoxMode.Date;

        else if (btnInfo.ID == "BtnUpEmail")
        {
            TbUpdate.TextMode = TextBoxMode.Email;
            TbUpdate.Attributes["placeholder"] = "Enter New " + btnInfo.Text;
        }
        else if (btnInfo.ID == "BtnUpGuardNum")
        {
            TbUpdate.TextMode = TextBoxMode.Number;
            TbUpdate.Attributes["placeholder"] = "Enter New " + btnInfo.Text;
        }
        else
        {
            TbUpdate.Attributes["placeholder"] = "Enter New " + btnInfo.Text;
            TbUpdate.TextMode = TextBoxMode.SingleLine;
        }

        Cache["DD_CHOICE"] = btnInfo.ID;
        DivDisplay.Visible = true;
    }



    /*=================================================================================> Delete Member <==========================================================================
 //*****************************************************************************************************************************************************************************/


    //Btn Click Event to call Delete Person Method
    protected void BtnDeleteMember_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteMember();
        }
        catch (Exception)
        {
            DivConnectionErr.Visible = true;
        }
    }


    //Deletes the person form the data base and all relationships assocciated with the Business
    private void DeleteMember()
    {
        var client = new RestClient(port);
        TempCustomer cust = (TempCustomer)Cache.Get("CUSTOMER_OBJ");
        try
        {
            var request = new RestRequest("deletePerson", Method.PUT);
            request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
            request.AddParameter("email", cust.email);
            request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

            IRestResponse response = client.Execute(request);

            //Deserialize the result into the class provided
            var jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
            ResponseMessage resObj = jsonObject as ResponseMessage;

            if (resObj.success == true)
            {
                DeleteImage(cust);
                HidePersonDetails();
                DivDeletedMember.InnerHtml = "Success! Customer: " + cust.name + " Deleted!";
            }

            else
                DivFailed.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
        }
        catch (Exception)
        {
            DivConnectionErr.Visible = true;
        }

    }

    //Hide All The Details of a person
    private void HidePersonDetails()
    {
        DivDeletedMember.Visible = true;
        BtnCheckin.Visible = false;
        DropDownHide.Visible = false;
        ImgPerson.Visible = false;

        LblJoined.Visible = false;
        ImgPerson.Visible = false;
        PersonDetails.Visible = false;
        DivJoined.Visible = false;
    }



    //Deletes The Image From The Database
    private void DeleteImage(TempCustomer cust)
    {
        string key = WebConfigurationManager.AppSettings["CLOUDINARY_API_KEY"];
        string secret = WebConfigurationManager.AppSettings["CLOUDINARY_API_SECRET"];
        string name = WebConfigurationManager.AppSettings["CLOUDINARY_API_NAME"];


        Account account = new Account(name, key, secret);
        Cloudinary cloudinary = new Cloudinary(account);


        var delParams = new DelResParams()
        {
            PublicIds = new List<string>() { cust.publicImgId },
            Invalidate = true
        };
        var delResult = cloudinary.DeleteResources(delParams);
    }


}