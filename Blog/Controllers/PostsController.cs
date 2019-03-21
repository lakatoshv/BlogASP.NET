﻿using Blog.Models;
using Blog.Services.Posts;
using Blog.Services.Posts.Interfaces;
using Blog.ViewModels.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class PostsController : Controller
    {
        private PostsService _postsService = new PostsService();
        private BlogContext _db = new BlogContext();
        
        // GET: Posts
        public ActionResult Index()
        {
            var posts = _postsService.GetPosts();
            return View(posts);
        }
        // GET: Posts/Show/5
        public ActionResult Show(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            var postModel = _postsService.GetPost(id.Value);
            if(postModel == null) return RedirectToAction("Index", "Posts");
            return View(postModel);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            if(!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            else return View();
        }

        // POST: Posts/Create
        [HttpPost]
        public ActionResult Create(Post post)
        {
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    post.CreatedAt = DateTime.Now;
                    post.Author = User.Identity.GetUserId();
                    var result = _db.Posts.Add(post);
                    _db.SaveChanges();
                    return RedirectToAction("index", "Posts");
                }
                return null;
            }
            catch
            {
                return View();
            }
        }

        // GET: Like/5
        public ActionResult Like(int? id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                PostViewModel postModel = new PostViewModel();
                postModel.post = _db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
                postModel.post.Likes++;
                _db.Entry(postModel.post).State = EntityState.Modified;
                _db.SaveChanges();

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
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                PostViewModel postModel = new PostViewModel();
                postModel.post = _db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
                postModel.post.Dislikes++;
                _db.Entry(postModel.post).State = EntityState.Modified;
                _db.SaveChanges();

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
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            var postModel = _postsService.GetPost(id.Value);
            return View(postModel.post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, Post editedPost)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                var postModel = _postsService.GetPost(id.Value);
                editedPost.Author = postModel.post.Author;
                editedPost.Likes = postModel.post.Likes;
                editedPost.Dislikes = postModel.post.Dislikes;
                editedPost.Seen = postModel.post.Seen;
                editedPost.CreatedAt = DateTime.Now;
                _db.Entry(editedPost).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: Posts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                BlogContext db = new BlogContext();
                Post postForDelete = db.Posts.Where(post => post.Id.Equals(id)).FirstOrDefault();
                db.Posts.Remove(postForDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
