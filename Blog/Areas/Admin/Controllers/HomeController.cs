using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using Blog.Services.Identity;
using System.Web.Mvc;
using Blog.Areas.Admin.ViewModels;
using BLog.Data;
using Microsoft.AspNet.Identity.Owin;
using Blog.Services.Posts.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        /// <summary>
        /// The user manager.
        /// </summary>
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
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

        /// <summary>
        /// The role manager.
        /// </summary>
        private RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Gets the role manager.
        /// </summary>
        /// <value>
        /// The role manager.
        /// </value>
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BlogContext()));
            }
            private set
            {
                _roleManager = value;
            }
        }

        /// <summary>
        /// The posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// The tags service.
        /// </summary>
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
                Roles = await  RoleManager.Roles.ToListAsync(),
            });
    }
}