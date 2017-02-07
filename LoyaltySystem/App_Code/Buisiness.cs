using System.Collections.Generic;

public class Message
{
    public string password { get; set; }
    public string address { get; set; }
    public string phone { get; set; }
    public string name { get; set; }
    public string email { get; set; }
}

public class BuisinessRoot
{
    public List<Message> message { get; set; }
}

//stores the successfull logged in user
public class BizCred
{
    public bool success { get; set; }
    public string name { get; set; }
    public string message { get; set; }
}

public class Company
{
    public string password { get; set; }
    public string address { get; set; }
    public string phone { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string emergencyNum { get; set; }
}