using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using RestSharp;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using System.Globalization;

public partial class _Default : System.Web.UI.Page
{
    //private string port = WebConfigurationManager.AppSettings["LOCAL_PORT"];
    private string port = WebConfigurationManager.AppSettings["API_PORT"];

    public class VisitedData
    {
        public List<DataSet> data { get; set; }
    }

    public class DataSet
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



    protected void BtnTopVisited_Click(object sender, EventArgs e)
    {
        try
        {
            HtmlButton btn = sender as HtmlButton;
            TopVisited(btn);
        }
        catch (Exception)
        {

        }
    }


    //-------------------------------- Top 10 Visited / Least Recently Visited-----------------------------------------------------
    private void TopVisited(HtmlButton btn)
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
            DisplayTopVisited(btn, custObjList.message);
        }
    }

    //Displays the Person(s) with the highest Visited amount and generates the Html
    private void DisplayTopVisited(HtmlButton btn, List<TempCustomer> custObjList)
    {
        //Sort List By visited in desc order
        List<TempCustomer> SortedList = new List<TempCustomer>();

        //Sort the list by converting the string to a number, important as str "11" < str "2"
        if (btn.ID == "BtnTopVisited")
        {
            SortedList = custObjList.OrderByDescending(o => Convert.ToInt32(o.visited)).ToList();
            modalHeader.InnerHtml = "<h1 class=\"text-center\" style=\"color: goldenrod\">Top 10 Visitors</h1>";
        }
        else
        {
            //Sort in Assending order the datesVisited ThenByDescending visited total 
            SortedList = custObjList.OrderBy(lv => lv.datesVisited.Max()).ThenByDescending(o => Convert.ToInt32(o.visited)).ToList();        
            modalHeader.InnerHtml = "<h1 class=\"text-center\" style=\"color: goldenrod\">Top 10 Least Recent</h1>";
        }

        foreach (var cust in SortedList)
        {
            HtmlGenericControl licontrol = new HtmlGenericControl();
            licontrol.Attributes["class"] = "list-group-item justify-content-between text-capitalize";
            licontrol.TagName = "li";


            //Get When the Person was last here Max()
            if (btn.ID == "BtnLeastRecent")
            {
                List<string> HsList = cust.datesVisited.ToList();
                var lastVisited = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(HsList.Max()));
                licontrol.InnerText = lastVisited.ToString("dd/MM/yyyy") + " --> ";
            }
            licontrol.InnerHtml += cust.name;


            HtmlGenericControl spancontrol = new HtmlGenericControl();
            spancontrol.TagName = "span";
            spancontrol.Attributes["class"] = "badge badge-default badge-pill";
            spancontrol.InnerText = cust.visited;

            licontrol.Controls.Add(spancontrol);

            // add to the new div, not to the panel
            UlTopChart.Controls.Add(licontrol);
        }

        //Toggle The Top Ten Score Board
        Page.ClientScript.RegisterStartupScript(this.GetType(), "TopTen", "TopTenToggle()", true);
    }


    //-------------------------------- Visited  BarChart Request-----------------------------------------------------
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
        DataSet data = new DataSet();
        Dictionary<string, int> tempDic = new Dictionary<string, int>();

        var culture = CultureInfo.DefaultThreadCurrentCulture;
        var dateTimeInfo = DateTimeFormatInfo.GetInstance(culture);

        //Fills the Dictonary with all the months in the year by short name
        foreach (string name in dateTimeInfo.AbbreviatedMonthNames)
        {
            tempDic.Add(name, 0);
        }
        tempDic.Remove("");

        //Loop over everyone in the List then loop through all the dates they have been here, 
        //Adding 1 to every month they have been to the park
        foreach (var cust in custObjList)
        {
            foreach (var date in cust.datesVisited)
            {
                //Convert the Mill to DateTime
                var month = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(date));

                string CurrentMonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month.Month);

                //If the Dic Has the month already add 1 to the value else add a new month
                if (tempDic.ContainsKey(CurrentMonth))
                {
                    int value;
                    if (tempDic.TryGetValue(CurrentMonth, out value))
                        tempDic[CurrentMonth] = value + 1;
                }
            }
        }

        //Convert The Dictonary To A List of Objects
        _custList.data = tempDic.Select(p => new DataSet { Month = p.Key, Visits = p.Value }).ToList();

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

        }
    }
}