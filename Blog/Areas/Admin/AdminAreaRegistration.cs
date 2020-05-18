using System.Web.Mvc;

namespace Blog.Areas.Admin
{
    /// <summary>
    /// Admin area registration.
    /// </summary>
    /// <seealso cref="AreaRegistration" />
    public class AdminAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// Sets name of registered area.
        /// </summary>
        public override string AreaName => "Admin";

        /// <summary>
        /// Registers area in MVC ASP.NET application, using information in context area.
        /// </summary>
        /// <param name="context">Encapsulate information to register area.</param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { area = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Blog.Areas.Admin.Controllers" }
            );
        }
    }
}