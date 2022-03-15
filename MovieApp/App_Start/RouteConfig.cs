using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovieApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MovieApi",
                url: "Home/{action}/{id}/",
                defaults: new { controller = "Home", action = "GetMovie", id = "" },
                constraints: new { id = @"^[0-9]+$" }
            );

            routes.MapRoute(
               name: "RateMovieApi",
               url: "{controller}/{action}/{id}/{title}/{rating}",
               defaults: new { controller = "Home", action = "RateMovie", id= UrlParameter.Optional, title="", rating=""},
               constraints: new { id = @"^[0-9]+$" , title = @"^[a-zA-Z]+$",  rating = @"^[a-zA-Z]+$" }
           );

            routes.MapRoute(
                name: "HomeApiPaging",
                url: "Home/{page}",
                defaults: new { controller = "Home", action = "Index", page = "" },
                constraints: new { page = @"^[0-9]+$" }
            );
        }
    }
}
