using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Token
/// </summary>
 
//stores the token and token type res from AUTH0
public class Token
{
    public string access_token { get; set; }
    public string token_type { get; set; }
}