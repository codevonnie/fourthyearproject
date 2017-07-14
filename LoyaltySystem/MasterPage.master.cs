using System;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private UserSettings settings = new UserSettings();

    //used For Auth and Checking Who messages are for
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

    //Switch the text on the login/logout button
    public void logStatus(String check)
    {
        if (check == "true")
            BtnLogIn.Text = "LogOut";
        else
            BtnLogIn.Text = "LogIn";
    }


    //Check if logged in our out
    protected void LogOut_Click(object sender, EventArgs e)
    {
        if (check == "true")
        {
            Cache.Remove("Settings");
            Cache.Remove("CUSTOMER_OBJ");
            Response.Redirect("LoginPage.aspx", true);
        }
    }
}
