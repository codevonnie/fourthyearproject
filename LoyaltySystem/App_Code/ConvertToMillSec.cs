using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ConvertToMillSec
/// </summary>
public class ConvertToMillSec
{
    public Double DateToMillSec(DateTime date)
    {
        //http://stackoverflow.com/questions/5955883/datetimes-representation-in-milliseconds
        //Needed to get milliseconds for database

        string simpleDate = date.ToString("dd/MM/yyyy");
        DateTime dt = DateTime.ParseExact(simpleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        var mil = dt.ToUniversalTime().Subtract(
      new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      ).TotalMilliseconds;

        return mil;
    }
}