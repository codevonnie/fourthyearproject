using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_SignInPage : System.Web.UI.Page
{


    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];


    //SSL Cookie with Auth Token etc
    private UserSettings settings = new UserSettings();


    protected void Page_Load(object sender, EventArgs e)
    {
        init();
        try
        {
            GetUsrSettings();
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

        //Err,Success Messages
        DivSuccess.Visible = false;
        DivFailed.Visible = false;
        DivConnectionErr.Visible = false;
        DivSuccessCheckIn.Visible = false;

        DivFailedEmail.Visible = false;

        DivFailedScan.Visible = false;
    }


    private void GetUsrSettings()
    {
        //-------------------------------- CACHE AUTH Decrypted --------------------------------
        //Cache might be cleared so need to get another token
        settings._auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        settings._auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());
        settings._biz_Email = Decrypt.Base64Decode(Cache.Get("BizEmail").ToString());
    }


    //-------------------------------- Btn Check Member Click Event -------------------------------------------
    protected void BtnCheckMember_Click(object sender, EventArgs e)
    {
        //Check that something was entered into the text box first 
        try
        {
            var custObj = ParseQRCode();
            FindPerson(custObj);
        }
        catch (Exception)
        {
            DivFailedScan.Visible = true;
        }
    }


    //-------------------------------- Parse QR Code ----------------------------------------------------------
    /* Mehtod parses the QR Code input into a TempCustomer,
     */
    private TempCustomer ParseQRCode()
    {
        var custObj = new TempCustomer();
        var custObject = JsonConvert.DeserializeObject<TempCustomer>(TbQRCode.Text);
        custObj = custObject as TempCustomer;
        return custObj;
    }


    //-------------------------------- Find Person -----------------------------------------------------
    //Send request to api and find the person
    private void FindPerson(TempCustomer cust)
    {
        var client = new RestClient(port);

        try
        {
            var request = new RestRequest("api/findPerson", Method.POST);
            request.AddHeader("Authorization", settings._auth_Type + " " + settings._auth_Token);
            request.AddParameter("email", cust.email);
            request.AddParameter("bEmail", settings._biz_Email);

            IRestResponse response = client.Execute(request);

            //Deserialize the result into the class provided
            var jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
            ResponseMessage resObj = jsonObject as ResponseMessage;

            if (resObj.success == true)
            {
                //Person was found Display to the Company
                displayPerson(cust);

                //Cached customer obj with timeout
                Cache.Insert("CUSTOMER_OBJ", cust, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
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
        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.joined.ToString()));
        LblName.Text = cust.name;
        LblJoined.Text = date.ToString("dd/MM/yyyy");
        LblAge.Text = CalculateAge(cust.dob.ToString());
        LblEmail.Text = cust.email;

        //Dont Display Guardian Name/Num if Null
        if (cust.guardianName.ToString() == "null" || cust.guardianNum.ToString() == "null")
            HideGuard.Visible = false;

        //Dont Display membership if Null
        if (cust.membership.ToString() == "null")
            HideMember.Visible = false;

        LblGuardNum.Text = cust.guardianNum.ToString();
        LblGuardName.Text = cust.guardianName.ToString();

        LblIceName.Text = cust.iceName;
        LblIceNum.Text = cust.icePhone;
        LblMember.Text = cust.membership;

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
            DivFailed.Visible = true;
        }
    }

    //-------------------------------- UpdatePerson -----------------------------------------------------------
    //Send request to api and find the person 
    private void UpdatePerson(object sender)
    {
        Button btn = sender as Button;
        TempCustomer cust;

        var client = new RestClient(port);

        if (btn.ID == "BtnCheckin")
            cust = (TempCustomer)Cache.Get("CUSTOMER_OBJ");
        else
            cust = UpdateCustObj();

        ConvertToMillSec convert = new ConvertToMillSec();

        var request = new RestRequest("api/updatePerson", Method.PUT);
        request.AddHeader("Authorization", settings._auth_Type + " " + settings._auth_Token);
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

        if (cust.membership != "null")
        {
            DateTime date = Convert.ToDateTime(cust.membership);
            var mil = convert.DateToMillSec(date);
        }

        request.AddParameter("membership", cust.membership);


        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {
            //Person UPDATED
            if (btn.ID == "BtnCheckin")
                DivSuccessCheckIn.Visible = true;
            else
                DivSuccess.Visible = true;

            cust.email = cust.tempEmail;
        }
        else
            DivFailedEmail.Visible = true;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }

    //Switch on the Person Property that they want to change
    private TempCustomer UpdateCustObj()
    {
        String choice = Cache.Get("DD_CHOICE").ToString();
        TempCustomer temp = (TempCustomer)Cache.Get("CUSTOMER_OBJ");

        temp.tempEmail = temp.email;

        switch (choice)
        {
            case "BtnUpName":
                temp.name = TbUpdate.Text.ToString();
                LblName.Text = TbUpdate.Text.ToString();
                break;

            case "BtnUpGuardName":
                temp.guardianName = TbUpdate.Text.ToString();
                LblGuardName.Text = TbUpdate.Text.ToString();
                break;

            case "BtnUpGuardNum":
                temp.guardianNum = TbUpdate.Text.ToString();
                LblGuardNum.Text = TbUpdate.Text.ToString();
                break;

            case "BtnUpEmail":
                temp.tempEmail = TbUpdate.Text.ToString();
                LblEmail.Text = TbUpdate.Text.ToString();
                break;

            case "BthUpMembershipEndDate":
                DateTime date = Convert.ToDateTime(TbUpdate.Text.ToString());
                temp.membership = TbUpdate.Text;
                LblMember.Text = date.ToString("dd/MM/yyyy");
                break;
        }
        return temp;
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
        else
            TbUpdate.Attributes["placeholder"] = "Enter New " + btnInfo.Text;

        Cache["DD_CHOICE"] = btnInfo.ID;
        DivDisplay.Visible = true;
    }


}