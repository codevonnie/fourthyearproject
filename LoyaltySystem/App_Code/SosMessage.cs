using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for SosMessage
/// </summary>



public class SosMessage
{
    public string status { get; set; }
    public string message { get; set; }
    public string email { get; set; }
    public string longitude { get; set; }
    public string latitude { get; set; }
    public string business { get; set; }
    public string name { get; set; }

    /*BODY KEYS:
* - email
* - latitude
* - longitude
* - message
* - business
* - status
* - name
*/
}
