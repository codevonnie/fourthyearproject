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
    private string _auth_Token = "";
    private string _auth_Type = "";

    public class ResponseMessage
    {
        public bool success { get; set; }
        public string message { get; set; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        Page.SetFocus(TbQRCode);//Refocus on InputBox
       
        divDisplay.Visible = false;
        DivSuccess.Visible = false;
        DivFailed.Visible = false;

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
        //-------------------------------- CACHE AUTH Decrypted --------------------------------
        //Cache might be cleared so need to get another token
        _auth_Token = Decrypt.Base64Decode(Cache.Get("AuthToken").ToString());
        _auth_Type = Decrypt.Base64Decode(Cache.Get("AuthType").ToString());
    }


    //-------------------------------- Btn Check Member Click Event --------------------*FIX*------------
    protected void BtnCheckMember_Click(object sender, EventArgs e)
    {
        //Check that something was entered into the text box first 
        var custObj = ParseQRCode();
        FindPerson(custObj);

        //WORKING 100%
       // ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }

    //-------------------------------- Update Person Info Click Event ----------------*FIX*----
    protected void UpdatePersonInfo_Click(object sender, EventArgs e)
    {
        UpdatePerson(); //check to see if text entered 

        TbUpdate.Text = "";

        //******* NOT NEEDED TESTING ONLY *******
        //divDisplay.Visible = true;
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }

    //-------------------------------- Update Choice Click Event --------------------*FIX*----
    /* Method Gets the users choice from the DropDown box when a person has Arrived, located in the "Bootstrap Modal"
     * Displays a "Update textbox" that updates its placeholder based on the Choice made.
     */
    protected void UpdateChoice_Click(object sender, EventArgs e)
    {
        Button btnInfo = (Button)sender;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
        TbUpdate.Focus();
        TbUpdate.Attributes["placeholder"] = "Enter New " + btnInfo.Text;

        Cache["DD_CHOICE"] = btnInfo.Text.ToString();
        divDisplay.Visible = true;
    }

    protected void BtnCheckPersonIn_Click(object sender, EventArgs e)
    {
        //SEND POST REQUEST AND UPDATE PERSON'S VIST COUNTER / LAST DATE VISITED
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }


    //-------------------------------- Parse QR Code --------------------*FIX*-----------------
    /* Mehtod parses the QR Code input into a TempCustomer,
     */
    private TempCustomer ParseQRCode()
    {
        var custObj = new TempCustomer();
        try
        {
            dynamic custObject = JsonConvert.DeserializeObject<TempCustomer>(TbQRCode.Text);
            custObj = custObject as TempCustomer;
        }
        catch (Exception)
        {
            // Catch unknown formats eg wrong QRcode or person id
            //DISPLAY MODAL WITH ERROR MESSAGE OR SHOW BELOW WARNING MESSAGE
        }
        return custObj;
    }


    //-------------------------------- Find Person --------------------*FIX*-------------------
    //Send request to api and find the person
    private void FindPerson(TempCustomer cust)
    {
        var client = new RestClient(port);

        var request = new RestRequest("api/findPerson", Method.POST);
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);
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


        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {
            //Person was found Display to the Company
            displayPerson(cust);

            //Cached customer obj with timeout
            Cache.Insert("CUSTOMER_OBJ", cust, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
        }
        TbQRCode.Text = "";//Reset Text
    }


    private TempCustomer UpdateCustObj()
    {
        String choice = Cache.Get("DD_CHOICE").ToString();
        TempCustomer temp = (TempCustomer)Cache.Get("CUSTOMER_OBJ");
        switch (choice)
        {
            case "Name":
                temp.name = TbUpdate.Text.ToString();
                LblName.Text = TbUpdate.Text.ToString();
                break;
            case "Guardian Name":
                temp.guardianName = TbUpdate.Text.ToString();
                LblGuardName.Text = TbUpdate.Text.ToString();
                break;
            case "Guardian Number":
                temp.guardianNum = TbUpdate.Text.ToString();
                LblGuardNum.Text = TbUpdate.Text.ToString();
                break;
        }
        return temp;
    }


    //-------------------------------- UpdatePerson ------------------------ *FIX* ------------
    //Send request to api and find the person 
    private void UpdatePerson()
    {
        var client = new RestClient(port);
        TempCustomer cust = UpdateCustObj();

        var request = new RestRequest("api/updatePerson", Method.PUT);
        request.AddHeader("Authorization", _auth_Type + " " + _auth_Token);
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

        IRestResponse response = client.Execute(request);

        //Deserialize the result into the class provided
        dynamic jsonObject = JsonConvert.DeserializeObject<ResponseMessage>(response.Content);
        var resObj = jsonObject as ResponseMessage;

        if (resObj.success == true)
        {
            //Person UPDATED
            DivSuccess.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
        }
        else
        {
            DivFailed.Visible = true;
        }
    }

    //-------------------------------- Calculate Age ------------------------------------------
    private string CalculateAge(string dob)
    {
        //Might need validation *******
        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(dob));
        // Save today's date.
        var today = DateTime.Today;

        return (today.Year - date.Year).ToString();
    }


    //-------------------------------- Display Person ------------------------------*FIX*------------
    private void displayPerson(TempCustomer cust)
    {
        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.joined.ToString()));
        LblName.Text = cust.name;
        LblJoined.Text = date.ToString("d/MM/yyyy") ;
        LblAge.Text = CalculateAge(cust.dob.ToString());

        if (cust.guardianName.ToString() == "null" || cust.guardianNum.ToString() == "null")
        {
            HideGuard.Visible = false;
        }

        LblGuardNum.Text = cust.guardianNum.ToString();
        LblGuardName.Text = cust.guardianName.ToString();

        LblIceName.Text = cust.iceName;
        LblIceNum.Text = cust.icePhone;

        LblMember.Text = "TEMP VAL";

        ImgPerson.ImageUrl = cust.imgUrl;

        //Trigger the JS 
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#myModal').modal('show');</script>", false);
    }


}