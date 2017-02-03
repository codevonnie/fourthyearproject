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
    private string check = "f";
    protected string MyProperty { get { return check; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            try
            {
                _biz_Name = Decrypt.Base64Decode(Cache.Get("BizName").ToString());//Check If Logged In
                check = "true";
                //init();
            }
            catch (Exception)
            {
            };
        }
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


    protected void LogOut_Click(object sender, EventArgs e)
    {
        Cache.Remove("BizName");
        Cache.Remove("Auth_LoggedIn");
        Cache.Remove("AuthToken");
        Cache.Remove("AuthType");
        Response.Redirect("LoginPage.aspx", true);
        //{"success":true,"name":"scott coyne","dob":"598060800000","address":"15 Bothar Stiofan knocknacarra","phone":"353860844550","iceName":"GFYS","icePhone":"353860844550","joined":"1486148575027","email":"scottcoyne@hotmail.com","imgUrl":"https://res.cloudinary.com/hlqysoka2/image/upload/v1486148576/frwa33d7p9ssy0ue8sfr.jpg","guardianName":"null","guardianNum":"null"}
    }
}
