using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Blog.Controllers" }
            );
            routes.MapRoute(
                name: "my-posts",
                url: "my-posts",
                defaults: new { controller = "Posts", action = "UserPosts", id = UrlParameter.Optional },
                namespaces: new[] { "Blog.Controllers" }
            );
        }
    }
}
