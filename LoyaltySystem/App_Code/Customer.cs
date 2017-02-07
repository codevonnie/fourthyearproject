using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Customer
/// </summary>

public class Customer
{
    public string name { get; set; }
    public string address { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public DateTime dob { get; set; }
    public string contactNumber { get; set; }
    public string icePhone { get; set; }
    public String iceName { get; set; }
    public DateTime date { get; set; }
    public DateTime joined { get; set; }//milliseconds ?
    public string guardianNum { get; set; }
    public string guardianName { get; set; }
    public string customerToken { get; set; }
    public string imgUrl { get; set; }
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
    public int visited { get; set; }
}