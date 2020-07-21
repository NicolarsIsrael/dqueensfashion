using DQueensFashion.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DQueensFashion
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.Add("ProductDetails", new GetSEOFriendlyRoute("Product/ProductDetails/{id}",
            //  new RouteValueDictionary(new { controller = "Product", action = "ProductDetails" }),
            //  new MvcRouteHandler()));

            //routes.Add("AdminProductDetails", new GetSEOFriendlyRoute("Admin/ProductDetails/{id}",
            //new RouteValueDictionary(new { controller = "Admin", action = "ProductDetails" }),
            //new MvcRouteHandler()));

            routes.MapRoute(
                "ProductDetails",
                "Product/ProductDetails/{name}-{id}",
                defaults: new { controller = "Product", action = "ProductDetails", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "AdminProductDetails",
                "Admin/ProductDetails/{name}-{id}",
                defaults: new { controller = "Admin", action = "ProductDetails", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
