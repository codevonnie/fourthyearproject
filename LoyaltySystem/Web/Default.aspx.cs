using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using System.Diagnostics;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //private string port = WebConfigurationManager.AppSettings["API_PORT"];
    // private System.Web.HttpCookie authCookie;

    private UserSettings settings = new UserSettings();
    private static List<TempCustomer> _tempCust;

    private void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                Cache.Insert("CUSTOMER_LIST", _tempCust, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
            }
            catch (Exception)
            {
                //list was null
            }
        }
        try
        {
            settings = Cache.Get("Settings") as UserSettings;
            if (settings._loggedIn == null || settings._biz_Email == null || settings._auth_Token == null || settings._auth_Type == null)
                return;
        }
        catch (Exception)
        {
            Response.Redirect("LoginPage.aspx", true);
        };
        // }
    }


    [System.Web.Services.WebMethod]
    public static Boolean ArivalsToList(List<TempCustomer> data)
    {
        _tempCust = data;
        return true;
    }



    //Surround in a try catch
    private void GetCustomers()
    {
        //var client = new RestClient(port);

        //var request = new RestRequest("businessMembers", Method.GET);  //----------------------Get Customers not Business
        //request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));

        //IRestResponse response = client.Execute(request);
        //var content = response.Content; // raw content as string

        ////Deserialize the result into the class provided
        //dynamic jsonObject = JsonConvert.DeserializeObject<BuisinessRoot>(response.Content);
        //var bizObj = jsonObject as BuisinessRoot;

        //bindData(bizObj);
    }


    private void GetAllCustomers(List<TempCustomer> custList)
    {
        foreach (var cust in custList)
        {

        }

        //< div class="col-xs-6 col-sm-3 placeholder">
        //    <img src = "data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" width="200" height="200" class="img-responsive" alt="Generic placeholder thumbnail" />
        //    <h4>Label</h4>
        //    <span class="text-muted">Something else</span>
        //</div>
    }


    private void PrintCustomer()
    {
        int count = 0;
        _tempCust = Cache.Get("CUSTOMER_LIST") as List<TempCustomer>;

        foreach (var cust in _tempCust)
        {
            for (int i = 0; i < 4; i++)
            {
                HtmlGenericControl divcontrol = new HtmlGenericControl();
                divcontrol.Attributes["class"] = "col-xs-6 col-sm-3 placeholder DisplayPersonBox";
                divcontrol.TagName = "Div";

                PeoplePlaceHolder.Controls.Add(divcontrol);

                Image img = new Image();
                img.CssClass = "img-responsive RoundImg";
                img.Height = 200;
                img.Width = 300;
                img.ImageUrl = cust.imgUrl;
                img.ID = count.ToString();

                Label lblName = new Label();
                lblName.Text = "<b>Name:</b> " + cust.name;

                Label lblDob = new Label();
                lblDob.Text = "<br/><b>DOB:</b> " + ConvertToDateTime(cust.dob);

                Label lblJoined = new Label();
                lblJoined.Text = "<br/><b>Joined:</b> " + ConvertToDateTime(cust.joined);

                // add to the new div, not to the panel
                divcontrol.Controls.Add(img); 
                divcontrol.Controls.Add(lblName);
                divcontrol.Controls.Add(lblDob);
                divcontrol.Controls.Add(lblJoined);
                count++;
            }
        }
        //< div class="col-xs-6 col-sm-3 placeholder">
        //    <img src = "data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" width="200" height="200" class="img-responsive" alt="Generic placeholder thumbnail" />
        //    <h4>Label</h4>
        //    <span class="text-muted">Something else</span>
        //</div>
    }

    private string ConvertToDateTime(string mill )
    {
        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(mill));
        return date.ToString("dd/MM/yyyy");
    }


    private void bindData(BuisinessRoot bizObj)
    {
        GridView1.DataSource = bizObj.message;//binds the Grid to the OBJECTS in the list
        GridView1.DataBind();
    }

    protected void BtnCurrentCustomers_Click(object sender, EventArgs e)
    {
        try
        {
            PrintCustomer();
        }
        catch (Exception)
        {
            //LIST WAS NULL
        }
    }
}