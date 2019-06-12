using Blog.Models;
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
using Blog.Core.Dtos;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;
using PagedList;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private PostsService _postsService = new PostsService();
        private BlogContext _db = new BlogContext();

        // GET: Posts
        public ActionResult Index(string search, string sortBy, string orderBy, int page = 1)
        {
            var sortParameters = new SortParametersDto()
            {
                OrderBy = orderBy ?? "asc",
                SortBy = sortBy ?? "Title",
                CurrentPage = page,
                PageSize = 10
            };
            var posts = _postsService.GetPosts(sortParameters, search);
            return View(posts);
        }
        // GET: Posts/Show/5
        public ActionResult Show(int? id, string sorts, int page = 1)
        {
            if (id == null) return RedirectToAction("Index", "Posts");

            var sortParameters = new SortParametersDto()
            {
                CurrentPage = page,
                PageSize = 10
            };

            var postModelToUpdate = _db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
            if (postModelToUpdate != null)
            {
                postModelToUpdate.Seen++;
                _db.Entry(postModelToUpdate).State = EntityState.Modified;
                _db.SaveChanges();
            }

            var postModel = _postsService.GetPostWithComments(id.Value, sortParameters);
            if(postModel == null) return RedirectToAction("Index", "Posts");

            return View(postModel);
        }

        [Authorize]
        public ActionResult MyPosts(string display, string sortBy, string orderBy, string search, int page = 1)
        {
            var posts = new PostsViewModel();
            
            var sortParameters = new SortParametersDto()
            {
                OrderBy = orderBy ?? "asc",
                SortBy = sortBy ?? "Title",
                CurrentPage = page,
                PageSize = 10,
                DisplayType = display ?? "list"
        };

            posts = _postsService.GetCurrentUserPosts(User.Identity.GetUserId(), sortParameters, search);
            posts.DisplayType = display ?? "list";

            return View(posts);
        }

        // GET: Posts/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Post postModel)
        {
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    postModel.CreatedAt = DateTime.Now;
                    postModel.Author = User.Identity.GetUserId();
                    var result = _db.Posts.Add(postModel);
                    _db.SaveChanges();

                    if (!postModel.Tags.IsNullOrWhiteSpace())
                    {
                        String[] tags = postModel.Tags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var tag in tags)
                        {
                            var tagForAdd = new Tag()
                            {
                                Title = tag,
                                PostId = postModel.Id
                            };
                            _db.Tags.Add(tagForAdd);
                            
                        }
                    }
                    _db.SaveChanges();
                    _db.Tags.Where(tag => tag.PostId.Equals(postModel.Id)).ForEach(
                        t =>
                        {
                            postModel.PostTags.Add(t);
                        }

                    );
                    _db.Entry(postModel).State = EntityState.Modified;
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
        [HttpGet]
        [Authorize]
        public ActionResult Like(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                PostViewModel postModel = new PostViewModel();
                postModel.Post = _db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
                postModel.Post.Likes++;
                _db.Entry(postModel.Post).State = EntityState.Modified;
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
        [HttpGet]
        [Authorize]
        public ActionResult Dislike(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                PostViewModel postModel = new PostViewModel();
                postModel.Post = _db.Posts.Where(post => post.Id.Equals(id.Value)).FirstOrDefault();
                postModel.Post.Dislikes++;
                _db.Entry(postModel.Post).State = EntityState.Modified;
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
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            var postModel = _postsService.GetPost(id.Value);
            return View(postModel.Post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? id, Post editedPost)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                var postModel = _postsService.GetPost(id.Value);
                if (!postModel.Post.Author.Equals(User.Identity.GetUserId()))
                    return RedirectToAction("Show/" + postModel.Post.Id, "Posts");

                editedPost.Author = postModel.Post.Author;
                editedPost.Likes = postModel.Post.Likes;
                editedPost.Dislikes = postModel.Post.Dislikes;
                editedPost.Seen = postModel.Post.Seen;
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
        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                BlogContext db = new BlogContext();
                Post postForDelete = db.Posts.Where(post => post.Id.Equals(id)).FirstOrDefault();
                if (!postForDelete.Author.Equals(User.Identity.GetUserId()))
                    return RedirectToAction("Index", "Posts");

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
