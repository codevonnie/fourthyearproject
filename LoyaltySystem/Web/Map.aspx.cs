using System;


public partial class Map : System.Web.UI.Page
{
    private UserSettings settings = new UserSettings();

    protected void Page_Load(object sender, EventArgs e)
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
}