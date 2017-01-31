using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Map : System.Web.UI.Page
{
    private static List<SosMessage> sosList = new List<SosMessage>();

    protected void Page_Load(object sender, EventArgs e)
    {
        var scriptManager = ScriptManager.GetCurrent(Page);
        int countMessages = 0;
        if (scriptManager == null) return;

        scriptManager.Scripts.Add(new ScriptReference { Path = "~/Scripts/CustGoogleJs.js" });
        foreach (var mess in sosList)
        {
            string messageNumKey = "MessagePin";

            double latitude  = Convert.ToDouble(mess.latitude.ToString());
            double longitude = Convert.ToDouble (mess.longitude.ToString());
            string message = mess.message;
            string name = mess.name;
            string status = mess.status;
            string business = mess.business;
            string email = mess.email;

            messageNumKey = messageNumKey + countMessages++;

            string PinMessage = "'<h3>"+name+"</h3>" +
                "<br/>Email: " + email +
                "<br/>Status: " + status +
                "<br/>Lon: " + longitude +
                "<br/>Lat: " + latitude +
                "<br/><br/>Message: " + message + "'";

            //Calls A JS function located in the scriptManager (registered above programatically)
            if(countMessages<2)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "locatePerson", "newLocation(" + latitude + "," + longitude + ");", true);


            ScriptManager.RegisterStartupScript(this, this.GetType(), messageNumKey, "newPin(" + latitude + "," + longitude + "," + PinMessage + ");", true);
        }
    }

    //HTTP Post - To the current page with the array of Json Object messages from the masterpage ajax click event
    [System.Web.Services.WebMethod]
    public static void MessagesToArray(List<SosMessage> data)
    {
        sosList = data;
    }

}