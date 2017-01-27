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
    protected void Page_Load(object sender, EventArgs e)
    {
        Main();

    }
    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8500);

        server.Start();
        Console.WriteLine("Server has started on 127.0.0.1:8500.{0}Waiting for a connection...", Environment.NewLine);

        TcpClient client = server.AcceptTcpClient();

        Console.WriteLine("A client connected.");
    }
}