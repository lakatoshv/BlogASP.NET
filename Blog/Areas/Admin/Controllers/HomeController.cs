using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        // GET: Admin/Home        
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}