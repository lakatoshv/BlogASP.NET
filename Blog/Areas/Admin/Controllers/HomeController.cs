using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Blog.Services.Identity;
using System.Web.Mvc;
using Blog.Areas.Admin.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Blog.Services.Posts.Interfaces;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private readonly IPostsService _postsService;
        private readonly ITagsService _tagsService;

        public HomeController(
            IPostsService postsService,
            ITagsService tagsService)
        {
            _postsService = postsService;
            _tagsService = tagsService;
        }

        // GET: Admin/Home        
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Index() => 
            View(new DashboardViewModel
            {
                Users = await UserManager.Users.ToListAsync(),
                Posts = await _postsService.GetAllAsync(),
                Tags = await _tagsService.GetAllAsync(),
            });
    }
}