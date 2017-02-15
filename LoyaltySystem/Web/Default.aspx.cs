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
using System.Globalization;

public partial class _Default : System.Web.UI.Page
{
    private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    //private string port = WebConfigurationManager.AppSettings["API_PORT"];
    // private System.Web.HttpCookie authCookie;

    public class VisitedData
    {
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string Month { get; set; }
        public int Visits { get; set; }
    }



    private static VisitedData _custList = new VisitedData();
    private static CustomersList _tempList = new CustomersList();


    protected string VisitData { get { return JsonConvert.SerializeObject(_custList.data); } }


    private UserSettings settings = new UserSettings();


    private void Page_Load(object sender, EventArgs e)
    {

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


    protected void BtnTopVisited_Click(object sender, EventArgs e)
    {
        try
        {
            TopVisited();
        }
        catch (Exception)
        {

        }
    }


    //-------------------------------- Top 10 Visited-----------------------------------------------------
    private void TopVisited()
    {
        var client = new RestClient(port);

        var request = new RestRequest("topTenVisited", Method.POST);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
        request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

        IRestResponse response = client.Execute(request);
        var content = response.Content;

        dynamic jsonObject = JsonConvert.DeserializeObject<CustomersList>(response.Content);
        var custObjList = jsonObject as CustomersList;

        if (custObjList.success == "true")
        {
            DisplayTopVisited(custObjList.message);
        }
    }


    private void DisplayTopVisited(List<TempCustomer> custObjList)
    {

        //Sort List By visited in desc order
        var sortedList = custObjList.OrderByDescending(i => i.visited).ToList();
        int count = 1;
        foreach (var cust in sortedList)
        {
            HtmlGenericControl licontrol = new HtmlGenericControl();
            licontrol.Attributes["class"] = "list-group-item justify-content-between text-capitalize";
            licontrol.TagName = "li";
            licontrol.InnerText = cust.name;

            //if (count % 2 == 0)
            //    licontrol.Attributes["class"] += " list-group-item-info";

            HtmlGenericControl spancontrol = new HtmlGenericControl();
            spancontrol.TagName = "span";
            spancontrol.Attributes["class"] = "badge badge-default badge-pill";
            spancontrol.InnerText = cust.visited;

            licontrol.Controls.Add(spancontrol);

            // add to the new div, not to the panel
            UlTopChart.Controls.Add(licontrol);

            count++;
        }

        //Toggle The Top Ten Score Board
        Page.ClientScript.RegisterStartupScript(this.GetType(), "TopTen", "TopTenToggle()", true);
    }




    //-------------------------------- Visited  BarChart -----------------------------------------------------
    private void VisitedBarChart()
    {
        var client = new RestClient(port);

        var request = new RestRequest("visitedTotal", Method.POST);
        request.AddHeader("Authorization", Decrypt.Base64Decode(settings._auth_Type.ToString()) + " " + Decrypt.Base64Decode(settings._auth_Token.ToString()));
        request.AddParameter("bEmail", Decrypt.Base64Decode(settings._biz_Email.ToString()));

        IRestResponse response = client.Execute(request);
        var content = response.Content;

        dynamic jsonObject = JsonConvert.DeserializeObject<CustomersList>(response.Content);
        var custObjList = jsonObject as CustomersList;

        if (custObjList.success == "true")
        {
            SortAllVisited(custObjList.message);
        }
    }

    /* Loop through the List of custObjList objects, and add the month / LastVisited to a Dictionary
     * Sort the List By Visits and then 
     */
    private void SortAllVisited(List<TempCustomer> custObjList)
    {
        Data data = new Data();
        Dictionary<string, int> tempDic = new Dictionary<string, int>();

        foreach (var cust in custObjList)
        {
            //Convert the Mill to DateTime
            var month = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(cust.lastVisited));

            //Get the current month by number eg 1-12
            string CurrentMonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month.Month);

            //If the Dic Has the month already add 1 to the value else add a new month
            if (tempDic.ContainsKey(CurrentMonth))
            {
                int value;
                if (tempDic.TryGetValue(CurrentMonth, out value))
                    tempDic[CurrentMonth] = value + 1;
            }
            else
                tempDic.Add(CurrentMonth, 0);

        }

        //Temp Data for chart
        tempDic.Add("Jan", 20);
        tempDic.Add("April", 54);
        tempDic.Add("Dec", 145);
        tempDic.Add("May", 45);
        tempDic.Add("June", 35);
        tempDic.Add("July", 75);

        //Convert The Dictonary To A List of Objects
        _custList.data = tempDic.Select(p => new Data { Month = p.Key, Visits = p.Value }).ToList();

        //Sort List By Visits Number
        _custList.data = _custList.data.OrderBy(x => x.Visits).ToList();

        //Register the js script
        Page.ClientScript.RegisterStartupScript(this.GetType(), "DisplayBarChart", "DisplayChart()", true);
    }


    protected void BtnBarChart_ServerClick(object sender, EventArgs e)
    {
        try
        {
            VisitedBarChart();
        }
        catch (Exception)
        {

            throw;
        }
    }
}