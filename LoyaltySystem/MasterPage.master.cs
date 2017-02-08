using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private UserSettings settings = new UserSettings();
    private string check;
    private string email;
    protected string LoggedInStatus { get { return check; } }
    protected string BizEmail { get { return email; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            settings = Cache.Get("Settings") as UserSettings;
            if (settings._loggedIn == null || settings._biz_Email == null || settings._auth_Token == null || settings._auth_Type == null)
                return;

            email = Decrypt.Base64Decode(settings._biz_Email.ToString());
            check = "true";
            logStatus(check);
        }
        catch (Exception)
        {
        };
    }

    //private void init()
    //{
    //    var scriptManager = ScriptManager.GetCurrent(Page);
    //    if (scriptManager == null) return;
    //    Boolean alreadyRegistered = false;

    //    ScriptReference SRef = new ScriptReference();
    //    SRef.Path = "~/Scripts/MessageServerListener.js";


    //        if (!scriptManager.Scripts.Contains(SRef))
    //        {
    //        scriptManager.Scripts.Add(new ScriptReference { Path = "~/Scripts/MessageServerListener.js" });
    //    }


    //}



    public void logStatus(String check)
    {
        if (check == "true")
            BtnLogIn.Text = "LogOut";
        else
            BtnLogIn.Text = "LogIn";
    }

    public String messageBoxNumber
    {
        get { return messageBoxCount.InnerText; }
        set { messageBoxCount.InnerText = value; }
    }


    protected void LogOut_Click(object sender, EventArgs e)
    {
        if (check == "true")
            Cache.Remove("Settings");
        else
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SendListToPage()", false);

        Response.Redirect("LoginPage.aspx", true);
    }
}
