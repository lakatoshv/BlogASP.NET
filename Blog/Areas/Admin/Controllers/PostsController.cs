using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Blog.Areas.Admin.Services.Posts;
using Blog.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PostsController : Controller
    {
        private readonly PostsService _postsService = new PostsService();
        private readonly BlogContext _db = new BlogContext();
        // GET: Admin/Posts
        [HttpGet]
        public ActionResult Index(string search)
        {
            var posts = _postsService.GetPosts(search);
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
        public ActionResult Create(Post postModel)
        {
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    postModel.CreatedAt = DateTime.Now;
                    postModel.Author = User.Identity.GetUserId();
                    _db.Posts.Add(postModel);
                    _db.SaveChanges();

                    if (!postModel.Tags.IsNullOrWhiteSpace())
                    {
                        String[] tags = postModel.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                    return RedirectToAction("index", "Posts", new { area = "Admin" });
                }
                return null;
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Posts/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return View();
        }

        // POST: Admin/Posts/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, Post editedPost)
        {
            return View();
        }

        // GET: Admin/Posts/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }
    }
}