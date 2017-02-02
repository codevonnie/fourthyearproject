﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Map : System.Web.UI.Page
{

    public class DeletePinByGeo
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

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


            string PinMessage = "'" +
                "<div class=\"MessageBoxMaxWidth text-capitalize\">" +
                "<h3 class=\"text-center\">" + name + "</h3>" +
                "<p><br/><b>Email</b>: " + email + "</p>" +
                "<p><br/><b>Status</b>: " + status + "</p>" +
                "<p><br/><b>Message</b>: " + message + "</p>" +
                "<button class=\"btn btn-danger btn-block\" type=\"button\" onclick=\"RemoveMessage()\">Delete Pin And Message</button>" +
                "</div >" +
                "'";

            //<button class="btn btn-danger btn-block" type="button" runat="server" onserverclick="myFunction">Delete Pin And Message</button>
            //<asp:button runat="server" text="Button" />

            //Only sets the Location of the first Message(centers map on that location) 2 because ++ on inner loop
            if (countMessages < 2)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "locatePerson", "newLocation(" + latitude + "," + longitude + ");", true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), messageNumKey, "newPin(" + latitude + "," + longitude + "," + PinMessage + ");", true);
        }
    }

    [System.Web.Services.WebMethod]
    public static void DeleteMessage(DeletePinByGeo data)
    {
        List<SosMessage> tempList = new List<SosMessage>();
        tempList.AddRange(sosList);

        string lng = Math.Round(data.lng, 6).ToString().Trim();
        string lat = Math.Round(data.lat, 6).ToString().Trim();

        foreach (var pin in sosList)
        {
            if (pin.latitude.Trim() == lat && pin.longitude.Trim() == lng)
            {
                tempList.Remove(pin);
            }
        }
        sosList = tempList;
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