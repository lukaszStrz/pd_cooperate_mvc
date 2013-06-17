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
               name: "AddUserRoute",
               url: "Groups/AddUser/{login}/{groupId}",
               defaults: new { controller = "Groups", action = "AddUser", login = UrlParameter.Optional, groupId = UrlParameter.Optional }
               );
            routes.MapRoute(
               name: "DeleteUserRoute",
               url: "Groups/DeleteUser/{login}/{groupId}",
               defaults: new { controller = "Groups", action = "DeleteUser", login = UrlParameter.Optional, groupId = UrlParameter.Optional }
               );
            routes.MapRoute(
               name: "AccountEditRoute",
               url: "Account/Edit/{login}",
               defaults: new { controller = "Account", action = "Edit", login = UrlParameter.Optional }
               );
            routes.MapRoute(
                name: "AddToGroupRoute",
                url: "Account/AddToGroup/{login}",
                defaults: new { controller = "Account", action = "AddToGroup", login = UrlParameter.Optional }
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