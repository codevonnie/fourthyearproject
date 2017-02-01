using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void logStatus(Boolean check)
    {
        if (check == false)
            logInBtn.InnerText = "LogIn";
        else
            logInBtn.InnerText = "LogOut";
    }

    public String messageBoxNumber
    {
        get { return messageBoxCount.InnerText; }
        set { messageBoxCount.InnerText = value; }
    }

}
