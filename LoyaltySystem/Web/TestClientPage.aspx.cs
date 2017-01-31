using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_TestClientPage : System.Web.UI.Page
{
    private static List<SosMessage> sosList = new List<SosMessage>();

    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (var mess in sosList)
        {
            TaMessageBox.Text += "\n Message: " + mess.message.ToString();
            TaMessageBox.Text += "\n Status: " + mess.status.ToString();
            TaMessageBox.Text += "\n Email: " + mess.email.ToString();
            TaMessageBox.Text += "\n Lonitude: " + mess.longitude.ToString();
            TaMessageBox.Text += "\n Latitude: " + mess.latitude.ToString();
            TaMessageBox.Text += "\n Business: " + mess.business.ToString();
            TaMessageBox.Text += "\n Name: " + mess.name.ToString() + "\n";
        }
    }


    //HTTP Post - To the current page with the array of Json Object messages from the masterpage ajax click event
    [System.Web.Services.WebMethod]
    public static void MessagesToArray(List<SosMessage> data)
    {
        sosList = data;
    }
}