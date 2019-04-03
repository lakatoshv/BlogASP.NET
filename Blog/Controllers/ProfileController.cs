using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Blog.ViewModels.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            BlogContext db = new BlogContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ProfileViewModel profile = new ProfileViewModel();
            var userId = User.Identity.GetUserId();
            profile.UserData = userManager.FindByIdAsync(userId).Result;

            profile.ProfileData = db.Profiles.Where(pr => pr.ApplicationUser.Equals(userId)).FirstOrDefault();
            profile.Posts = db.Posts.Where(post => post.Author.Equals(userId)).ToList(); ;
            return View(profile);
        }

        // GET: Profile/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            BlogContext db = new BlogContext();
            Profile profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(id)).FirstOrDefault();
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
