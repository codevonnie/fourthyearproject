using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private object _biz_Name = "";
    private string check;
    protected string MyProperty { get { return check; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
            try
            {
                _biz_Name = Decrypt.Base64Decode(Cache.Get("BizName").ToString());//Check If Logged In
                check = "true";
                logStatus(check);
                //init();
            }
            catch (Exception)
            {
            };
        //}
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
        {
            Cache.Remove("BizName");
            Cache.Remove("Auth_LoggedIn");
            Cache.Remove("AuthToken");
            Cache.Remove("AuthType");
            Cache.Remove("BizEmail");
            Response.Redirect("LoginPage.aspx", true);
        }
        else
        {
            Response.Redirect("LoginPage.aspx", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SendListToPage()", false);
        }
    }
}
