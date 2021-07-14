using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Areas.Admin.ViewModels;
using BLog.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Roles controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
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
        /// Gets all roles.
        /// </summary>
        /// <returns>ActionResult</returns>
        // GET: Admin/Roles
        public async Task<ActionResult> Index()
        {
            ViewBag.Controller = "Posts";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();

            var roles = await RoleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: Roles/Create
        /// <summary>
        /// Create role page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create() => View();

        // POST: Roles/Create
        /// <summary>
        /// Create role action.
        /// </summary>
        /// <param name="roleModel">roleModel.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(IdentityRole roleModel)
        {
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return null;
                }
                await RoleManager.CreateAsync(roleModel);
                return RedirectToAction("index", "Roles", new { area = "Admin" });
            }
            catch
            {
                return View();
            }
        }
    }
}