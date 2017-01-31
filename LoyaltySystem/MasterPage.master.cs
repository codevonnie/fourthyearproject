using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{

    public class SosMessage
    {
        public String status { get; set; }
        public String message { get; set; }
    }

    static SosMessage sosMess = new SosMessage();
    private static List<SosMessage> msgList = new List<SosMessage>();

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



    protected void GetMessage_click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalViewMessages", "<script>$('#myModal').modal('show');</script>", false);


        List<String> messageList = new List<string>();
        var check = hdnResultValue.Value;
        messageList.Add(check);

        foreach (string m in messageList)
        {
            sosMess.status = m.Substring(m.IndexOf("¬*¬"), m.IndexOf("¬**¬"));
            sosMess.message = m.Substring(m.IndexOf("¬**¬") + 3);
            msgList.Add(sosMess);
        }
    }






    protected void ClickEvent_ShowMessage(object sender, EventArgs e)
    {

        SosMessage sos;


        ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalMessages", "<script>$('#myModal').modal('show');</script>", false);





        List<String> messageList = new List<string>();
        var check = hdnResultValue.Value;
        messageList.Add(check);

        foreach (string m in messageList)
        {
            sosMess.status = m.Substring(m.IndexOf("¬*¬"), m.IndexOf("¬**¬"));
            sosMess.message = m.Substring(m.IndexOf("¬**¬") + 3);
            msgList.Add(sosMess);
        }
    }

}
