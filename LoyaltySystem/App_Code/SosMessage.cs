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
}

public class City
{
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    private int population;
    public int Population
    {
        get
        {
            return population;
        }
        set
        {
            population = value;
        }
    }
}