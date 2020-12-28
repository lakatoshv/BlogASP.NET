using System.Web.Mvc;

namespace Blog.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index() =>
            RedirectToAction("Index", "Posts");

        /// <summary>
        /// About this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult About() => View();

        /// <summary>
        /// Contacts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Contact() => View();
    }
}