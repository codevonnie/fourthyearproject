using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Map : System.Web.UI.Page
{
    private static List<SosMessage> sosList = new List<SosMessage>();
    private static int countMessages = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        countMessages = 0;
        var scriptManager = ScriptManager.GetCurrent(Page);
        if (scriptManager == null) return;

        scriptManager.Scripts.Add(new ScriptReference { Path = "~/Scripts/CustGoogleJs.js" });

        MapMessagesAsPins();

        //Calls A JS function located in the scriptManager (registered above programatically)
        if (countMessages < 1)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "geoLocationKey", "getLocation();", true);

    }

    private void MapMessagesAsPins()
    {

        Master.messageBoxNumber = sosList.Count().ToString();

        //For each message in the sosList, get the values and add a pin to the map at the Geolocation passed in
        foreach (var mess in sosList)
        {
            string messageNumKey = "MessagePin";

            double latitude = Convert.ToDouble(mess.latitude.ToString());
            double longitude = Convert.ToDouble(mess.longitude.ToString());
            string message = mess.message;
            string name = mess.name;
            string status = mess.status;
            string business = mess.business;
            string email = mess.email;

            messageNumKey = messageNumKey + countMessages++;

            string PinMessage = "'<h3>" + name + "</h3>" +
                "<br/>Email: " + email +
                "<br/>Status: " + status +
                "<br/>Lon: " + longitude +
                "<br/>Lat: " + latitude +
                "<br/><br/>Message: " + message + "'";

            //Only sets the Location of the first Message(centers map on that location) 2 because ++ on inner loop
            if (countMessages < 2)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "locatePerson", "newLocation(" + latitude + "," + longitude + ");", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), messageNumKey, "newPin(" + latitude + "," + longitude + "," + PinMessage + ");", true);
        }
    }


    /*HTTP Post - To the current page with the array of Json Object messages from the masterpage ajax click event
     * Objects Only Get Added to the List if they Dont Have The Same Name(Persons Name)
    */
    [System.Web.Services.WebMethod]
    public static void MessagesToArray(List<SosMessage> data)
    {
        //Adapted From http://stackoverflow.com/questions/9969612/how-to-combine-two-lists-without-duplication-possible-dup
        var uniqueList = sosList
            .Concat(data)
            .GroupBy(item => item.name)
            .Select(group => group.First())
            .ToArray();

        sosList = uniqueList.ToList();
    }

}