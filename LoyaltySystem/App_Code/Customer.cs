using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Customer
/// </summary>

public class CustomersList
{
    public string success { get; set; }
    public List<TempCustomer> message { get; set; }
}

public class TempCustomer
{
    public bool success { get; set; }
    public string name { get; set; }
    public string dob { get; set; }
    public string address { get; set; }
    public string phone { get; set; }
    public string iceName { get; set; }
    public string icePhone { get; set; }
    public string joined { get; set; }
    public string email { get; set; }
    public string tempEmail { get; set; }
    public string imgUrl { get; set; }
    public string guardianName { get; set; }
    public string guardianNum { get; set; }
    public string membership { get; set; }
    public string tempPwd { get; set; }
    public string visited { get; set; }
    public string publicImgId { get; set; } 
    public HashSet<string> datesVisited { get; set; }
}
