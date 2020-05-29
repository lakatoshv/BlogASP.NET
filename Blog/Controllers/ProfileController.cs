using System.Data;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLog.Data;
using Blog.Data.Models;
using Blog.Services.Interfaces;
using Blog.ViewModels.Users;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Blog.Controllers
{
    /// <summary>
    /// Profile controller.
    /// </summary>
    public class ProfileController : Controller
    {
        /// <summary>
        /// Profiles service.
        /// </summary>
        private readonly IProfilesService _profilesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="profilesService">The posts service.</param>
        public ProfileController(IProfilesService profilesService)
        {
            _profilesService = profilesService;
        }

        // GET: Profile
        /// <summary>
        /// Indexes the specified search.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var profile = await _profilesService.GetProfileWithPostsByUserId(User.Identity.GetUserId());
            
            return View(profile);
        }

        // GET: Profile/Details/5
        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var profile = await _profilesService.GetProfileWithPostsById(id.Value);
            
            return View(profile);
        }

        // GET: Profile/Edit/5
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var profile = await _profilesService.GetProfileByUserId(id.Value, User.Identity.GetUserId());
            
            return View(profile);
        }

        // POST: Profile/Edit/5
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The edited post.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, ProfileViewModel model)
        {
            var userId = User.Identity.GetUserId();
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var store = new UserStore<ApplicationUser>(new BlogContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var userModel = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            if (!model.UserData.Id.Equals(userId))
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                
                userModel.Email = model.UserData.Email;
                userModel.PhoneNumber = model.UserData.PhoneNumber;
                await userManager.UpdateAsync(userModel);
                
                // await _profilesService.UpdateProfile()
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            var identity = new ClaimsIdentity(User.Identity);

            // update claim value


            identity.RemoveClaim(identity.FindFirst("FirstName"));
            identity.RemoveClaim(identity.FindFirst("LastName"));
            identity.RemoveClaim(identity.FindFirst("ProfileImg"));
            identity.AddClaims(new[]
            {
                new Claim("FirstName", !model.UserData.FirstName.IsNullOrWhiteSpace() ? model.UserData.FirstName : ""),
                new Claim("LastName", !model.UserData.LastName.IsNullOrWhiteSpace() ? model.UserData.LastName : ""),
                new Claim("ProfileImg", !model.UserData.Profile.ProfileImage.IsNullOrWhiteSpace() ? model.UserData.Profile.ProfileImage : ""),
                new Claim("ProfileId", id.Value.ToString())
            });

            var authenticationManager = HttpContext.GetOwinContext().Authentication;

            authenticationManager.AuthenticationResponseGrant =
                new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );

            return RedirectToAction("Show/" + id, "Posts");
        }
    }
}
