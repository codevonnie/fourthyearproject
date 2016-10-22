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
