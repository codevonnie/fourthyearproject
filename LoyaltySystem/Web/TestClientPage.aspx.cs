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
    static List<SosMessage> sos = new List<SosMessage>();
    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (var mess in sos)
        {
            TaMessageBox.Text += "\nMessage: "+mess.message.ToString();
            TaMessageBox.Text += "\nStatus: "+mess.status.ToString()+"\n";
        }
    }


    /*Posts To the current page the Object Json messages from masterpage click event
*/
    //[System.Web.Services.WebMethod]
    //public static void GetCurrentTime(Array [] name)
    //{
    //    //sos.message = name;     
    //}

    [System.Web.Services.WebMethod]
    public static SosMessage GetCity(SosMessage city)
    {
        return city;
    }

    [System.Web.Services.WebMethod]
    public static void CityObjectArray(List<SosMessage> city)
    {
        sos = city;
        
        /*
         * email
         * lat
         * long
         * message
         * buisiness
         * status
         * 
         */ 

    }
}