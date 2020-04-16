using System.Web.Mvc;
using Blog.Data.Models;

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
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Posts");
        }

        /// <summary>
        /// About this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Contacts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Contact()
        {
            Message message = new Message();
            return View(message);
        }
    }
}