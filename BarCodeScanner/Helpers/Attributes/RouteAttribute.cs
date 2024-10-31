using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RouteAttribute : Attribute
    {
        public string Route { get; set; }
        public RouteAttribute()
        {
        }
        public RouteAttribute(string route)
        {
            Route = route;
        }
    }
}
