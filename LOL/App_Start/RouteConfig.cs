using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LOL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new
               {
                   controller = "Home",
                   action = "Index",
                   id = UrlParameter.Optional
              
               }

            );

            routes.MapRoute(
               name: "Add Actor (Film)",
               url: "Actings/Create/{subName}/{FilmId}",
               defaults: new
               {
                   controller = "Actings",
                   action = "Create",
                   FilmId  = UrlParameter.Optional
               },
               constraints: new
               {
                   //as long as this subname is given, the avbove rout applies
                   subName = "Film"
               }
               
            );
            routes.MapRoute(
               name: "Add Actor (Person)",
               url: "Actings/Create/{subName}/{PersonId}",
               defaults: new
               {
                   controller = "Actings",
                   action = "Create",
                   FilmId = UrlParameter.Optional
               },
               constraints: new
               {
                   //as long as this subname is given, the avbove rout applies
                   subName = "Person"
               }

            );
        }
    }
}
