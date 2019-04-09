using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Blog.ViewModels.Users;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Blog.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            BlogContext db = new BlogContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ProfileViewModel profile = new ProfileViewModel();
            var userId = User.Identity.GetUserId();
            profile.UserData = userManager.FindByIdAsync(userId).Result;

            profile.ProfileData = db.Profiles.Where(pr => pr.ApplicationUser.Equals(userId)).FirstOrDefault();
            profile.Posts = db.Posts.Where(post => post.Author.Equals(userId)).ToList();
            return View(profile);
        }

        // GET: Profile/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            BlogContext db = new BlogContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ProfileViewModel profile = new ProfileViewModel();

            profile.ProfileData = db.Profiles.Where(pr => pr.Id.Equals(id.Value)).FirstOrDefault();
            if(profile.ProfileData != null && profile.ProfileData.ApplicationUser.IsNullOrWhiteSpace()) return RedirectToAction("Index", "Posts");
            profile.Posts = db.Posts.Where(post => post.Author.Equals(profile.ProfileData.ApplicationUser)).ToList();
            profile.UserData = userManager.FindByIdAsync(profile.ProfileData.ApplicationUser).Result;

            return View(profile);
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null) return RedirectToAction("Index", "Posts");

            BlogContext db = new BlogContext();
            ProfileViewModel profile = new ProfileViewModel();
            profile.ProfileData = db.Profiles.Where(pr => pr.Id.Equals(id.Value)).FirstOrDefault();
            if (!profile.ProfileData.ApplicationUser.Equals(User.Identity.GetUserId())) return RedirectToAction("Index", "Posts");

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            profile.UserData = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            return View(profile);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int? id, ProfileViewModel model)
        {
            var userId = User.Identity.GetUserId();
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null) return RedirectToAction("Index", "Posts");

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var userModel = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            //if (!model.ProfileData.ApplicationUser.Equals(userId)) return RedirectToAction("Index", "Posts");
            BlogContext db = new BlogContext();

            try
            {
                
                userModel.Email = model.UserData.Email;
                userModel.PhoneNumber = model.UserData.PhoneNumber;
                IdentityResult result = await userManager.UpdateAsync(userModel);
                Profile profileModel = model.ProfileData;
                profileModel.ApplicationUser = userId;
                profileModel.Id = id.Value;

                db.Entry(profileModel).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
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

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
