using Blog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Blog.Core.Attributes;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly BlogContext _db = new BlogContext();

        // GET: Comments/Create
        [HttpGet]
        [Authorize]
        [CheckPermissionsToEditForComments]
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [CheckPermissionsToEditForComments]
        public ActionResult Create(Comment comment)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            try
            {
                comment.CreatedAt = DateTime.Now;
                comment.Author = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    _db.Comments.Add(comment);
                    _db.SaveChanges();
                    return RedirectToAction("Show/" + comment.PostID, "Posts");
                }
                return null;
            }
            catch
            {
                return View();
            }
        }

        // GET: Comments/Edit/5
        [HttpGet]
        [Authorize]
        [CheckPermissionsToEditForComments]
        public ActionResult Edit(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [CheckPermissionsToEditForComments]
        public ActionResult Edit(Comment comment)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            try
            {
                var originalComment = _db.Comments.FirstOrDefault(comm => comm.Id.Equals(comment.Id));
                if (originalComment != null && !originalComment.Author.Equals(User.Identity.GetUserId()))
                    return RedirectToAction("Show/" + comment.PostID, "Posts");

                comment.Author = User.Identity.GetUserId();
                comment.CreatedAt = DateTime.Now;
                _db.Entry(comment).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Show/" + comment.PostID, "Posts");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(comment);
            
            
        }

        /*
        // GET: Comments/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            return View();
        }
        */

        // POST: Comments/Delete/5
        [HttpPost]
        [Authorize]
        [CheckPermissionsToEditForComments]
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Comment comment = _db.Comments.FirstOrDefault(comm => comm.Id.Equals(id));
                if (comment != null && !comment.Author.Equals(User.Identity.GetUserId()))
                    return RedirectToAction("Show/" + comment.PostID, "Posts");

                if (comment != null)
                {
                    _db.Comments.Remove(comment);
                    _db.SaveChanges();
                }

                if (comment != null) return RedirectToAction("Show/" + comment.PostID, "Posts");
            }
            catch
            {
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index", "Posts");
        }
    }
}
