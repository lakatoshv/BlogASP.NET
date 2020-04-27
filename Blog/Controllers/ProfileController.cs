using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blog.ViewModels.Users;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
    /// <summary>
    /// Profile controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class ProfileController : Controller
    {
        private readonly IProfilesService _profilesService;

        public ProfileController(IProfilesService profilesService)
        {
            _profilesService = profilesService;
        }

        // GET: Profile        
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.GetUserId();
            return View(await _profilesService.GetCurrentUserProfileWithPosts(userId));
        }

        // GET: Profile/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var profile = await _profilesService.GetProfileWithPosts(id.Value);

            if(profile == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            return View(profile);
        }

        // GET: Profile/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null) return RedirectToAction("Index", "Posts");

            var profile = await _profilesService.GetCurrentUserProfileById(id.Value, User.Identity.GetUserId());
            if(profile == null)
                return RedirectToAction("Index", "Posts");
            return View(profile);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int? id, ProfileViewModel model)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var userId = User.Identity.GetUserId();
                await _profilesService.UpdateProfile(userId, model.UserData.Email, model.UserData.PhoneNumber, id.Value, model.ProfileData);
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
                new Claim("FirstName", !model.ProfileData.FirstName.IsNullOrWhiteSpace() ? model.ProfileData.FirstName : ""),
                new Claim("LastName", !model.ProfileData.LastName.IsNullOrWhiteSpace() ? model.ProfileData.LastName : ""),
                new Claim("ProfileImg", !model.ProfileData.ProfileImg.IsNullOrWhiteSpace() ? model.ProfileData.ProfileImg : ""),
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
