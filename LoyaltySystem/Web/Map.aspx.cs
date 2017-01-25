using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class UserMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var scriptManager = ScriptManager.GetCurrent(Page);

        if (scriptManager == null) return;

        scriptManager.Scripts.Add(new ScriptReference { Path = "~/Scripts/CustGoogleJs.js" });

        double latitude = -9.0151516; //need to pass this to JS var
        double longitude = 53.276164; //need to pass this to JS var
        string userMessage = "Help iv Broken My Toe!!";
        string message = "'<h3>Location Info</h3>" +
            "<br/>Lon: " + longitude +
            "<br/>Lat: " + latitude  +
            "<br/><br/>Message: " + userMessage+"'";

        //Calls A JS function located in the scriptManager (registered above programatically)
        ScriptManager.RegisterStartupScript(this, this.GetType(), "locatePerson", "newLocation(" + longitude + ","+ latitude+");", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "newPin", "newPin(" + longitude + "," + latitude + ","+ message+");", true);

    }

}