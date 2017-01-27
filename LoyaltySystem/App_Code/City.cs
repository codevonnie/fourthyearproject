using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


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