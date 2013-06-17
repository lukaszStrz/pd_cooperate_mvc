using System.Web.Mvc;
using System.Web.Routing;

namespace Cooperate_mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AccountDetailsRoute",
                url: "Account/Details/{login}",
                defaults: new { controller = "Account", action = "Details", login = UrlParameter.Optional }
                );
            routes.MapRoute(
               name: "AccountEditRoute",
               url: "Account/Edit/{login}",
               defaults: new { controller = "Account", action = "Edit", login = UrlParameter.Optional }
               );
            routes.MapRoute(
               name: "ChatRoute",
               url: "Messages/Chat/{login}",
               defaults: new { controller = "Messages", action = "Chat", login = UrlParameter.Optional }
               );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}