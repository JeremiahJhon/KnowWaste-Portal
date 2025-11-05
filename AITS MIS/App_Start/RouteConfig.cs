using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UCOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "NewRoute",
                url: "New/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Knowwaste.Areas.New.Controllers" } // 👈 point to the right controller
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "UCOnline.Controllers" } // 👈 point to the right controller
            );
            routes.MapRoute(
                name: "DataSpecial",
                url: "Data/",
                defaults: new { controller = "Data", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
