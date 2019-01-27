using Blog.Models;
using Blog.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        BlogContext db = new BlogContext();
        // GET: Posts
        public ActionResult Index()
        {
            ViewBag.Message = db.Posts.ToList();
            return View(db.Posts.ToList());
        }
        // GET: Posts/Show/5
        public ActionResult Show(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            PostViewModel postModel = new PostViewModel();
            postModel.post = db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
            postModel.comments = db.Comments.Where(comment => comment.PostID.Equals(id.Value)).ToList();
            return View(postModel);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
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

        // GET: Like/5
        public ActionResult Like(int? id)
        {
            if(id == null) return RedirectToAction("Index", "Posts");
            try
            {
                PostViewModel postModel = new PostViewModel();
                postModel.post = db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
                postModel.post.Likes++;
                db.Entry(postModel.post).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Dislike/5
        public ActionResult Dislike(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                PostViewModel postModel = new PostViewModel();
                postModel.post = db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
                postModel.post.Dislikes++;
                db.Entry(postModel.post).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Posts/Edit/5
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

        // GET: Posts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
