using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null, // Only matches the empty URL (i.e. /)
                null,
                defaults: new { controller = "Product", action = "List", category = (string)null, page = 1 }
            );

            routes.MapRoute(
                null,
                url: "Page{page}",
                defaults: new { controller = "Product", action = "List", category = (string)null },
                constraints: new { page = @"\d+" } //page must be numerical
            );

            routes.MapRoute(
               null,
               url: "{category}",
               defaults: new { controller = "Product", action = "List", page = 1 }
            );

            routes.MapRoute(
                null,
                url: "{category}/Page{page}",
                defaults: new { controller = "Product", action = "List" },
                constraints: new { page = @"\d+" } //page must be numerical
            );

            routes.MapRoute(
                null,
                url: "{controller}/{action}"
            );
        }
    }
}
