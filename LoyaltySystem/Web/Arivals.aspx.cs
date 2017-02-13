using Newtonsoft.Json;
using RestSharp;
using System;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;

using System.IO;
using System.Web;
using System.Web.Security;

using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class Arivals : System.Web.UI.Page
{
    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //private string port = WebConfigurationManager.AppSettings["API_PORT"];

    private TempCustomer _TempCust = new TempCustomer();
    private Boolean _newCust;
    private string _custJson="null";

   
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
        Membership.Visible = false;

        //Err,Success Messages
        DivSuccess.Visible = false;
        DivFailed.Visible = false;
        DivConnectionErr.Visible = false;
        DivSuccessCheckIn.Visible = false;

        DivFailedEmail.Visible = false;

        DivFailedScan.Visible = false;
    }


    //-------------------------------- Btn Check Member Click Event -------------------------------------------
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
            Membership.Visible = true;
            var memberDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.membership.ToString()));
            LblMember.Text = memberDate.ToString("dd/MM/yyyy");
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





    //=================================================================================> Update Methods <==========================================================================


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

        var client = new RestClient(port);

        //Check To see if the person is just checking in
        if (btn.ID == "BtnCheckin")
            cust.tempEmail = cust.email;
        else
        {
            if (btn.ID == "BtnRemoveGuard" || btn.ID == "BtnRemoveMembership")
                Cache["DD_CHOICE"] = btn.ID;
            cust = UpdateCustObj();
        }

        ConvertToMillSec convert = new ConvertToMillSec();

        var request = new RestRequest("updatePerson", Method.PUT);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
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
        request.AddParameter("tempEmail", cust.tempEmail);

        if (btn.ID == "BtnCheckin")
            cust.visited++;

        request.AddParameter("visited", cust.visited);

        if (cust.membership != "0") // Could have a Membership Already so maby a function to check if membership is out-of-date
        {
            var memberDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.membership.ToString()));

            var mil = convert.DateToMillSec(memberDate);
            cust.membership = mil.ToString();//Update the Membership with a new Date (Miliseconds)
            Membership.Visible = true;
        }

        request.AddParameter("membership", cust.membership);

        //SendRequest
        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {

            if ((Boolean)Cache.Get("CheckingIn")==true)
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

                Membership.Visible = false;
                break;

            case "BtnUpEmail":
                tempCust.tempEmail = TbUpdate.Text.ToString();
                LblEmail.Text = TbUpdate.Text.ToString();
                break;

            case "BntResetPwd":
                //string password = Membership.GeneratePassword(6, 3);
                //tempCust.tempPwd = password;
                break;

            case "BthUpMembershipEndDate":
                ConvertToMillSec convert = new ConvertToMillSec();
                DateTime date = Convert.ToDateTime(TbUpdate.Text.ToString());
                var memMill = convert.DateToMillSec(date);

                tempCust.membership = memMill.ToString();
                Membership.Visible = true;
                LblMember.Text = date.ToString("dd/MM/yyyy");
                break;
        }

        Cache.Insert("CUSTOMER_OBJ", tempCust, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10));

        return tempCust;
    }


    //-------------------------------- Update Choice Click Event ---------------------------------------
    /* Method Gets the users choice from the DropDown box when a person has Arrived, located in the "Bootstrap Modal"
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



    //=================================================================================> Local Storage <==========================================================================

    private void storePeople()
    {

    }


}