using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        BlogContext db = new BlogContext();
        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                comment.CreatedAt = DateTime.Now;
                
                if (ModelState.IsValid)
                {
                    var result = db.Comments.Add(comment);
                    db.SaveChanges();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (comment.Id == null && comment.PostID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                comment.CreatedAt = DateTime.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Show/" + comment.PostID, "Posts");
            }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comments/Delete/5
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Comment commentForDelete)
        {
            if (commentForDelete.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Comment comment = db.Comments.Where(comm => comm.Id.Equals(commentForDelete.Id)).FirstOrDefault();
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Show/" + comment.PostID, "Posts");
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index");
        }
    }
}
