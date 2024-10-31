using BarCodeScanner.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Helpers
{
    internal class RouteRegistrator
    {
        public void RegisterPagesRoutes(IEnumerable<Type> pageTypes)
        {
            var baseType = typeof(ContentPage);
            foreach (var type in pageTypes)
            {
                var routeAttr = type.GetCustomAttribute<RouteAttribute>();
                if (type.BaseType == baseType && routeAttr != null)
                {
                    if (string.IsNullOrWhiteSpace(routeAttr.Route))
                    {
                        Routing.RegisterRoute(type.Name.Replace("Page", "").Replace("View", ""), type);
                        continue;
                    }
                    Routing.RegisterRoute(routeAttr.Route, type);
                }
            }
        }



    }
}
