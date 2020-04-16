using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Core.Enums;
using Blog.Data.Models;
using Blog.Services.Posts;
using Microsoft.AspNet.Identity;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PostsController : Controller
    {
        /// <summary>
        /// Posts service.
        /// </summary>
        private readonly PostsService _postsService;

        /// <summary>
        /// Initializes static members of the <see cref="PostsController"/> class.
        /// </summary>
        public PostsController()
        {
            _postsService = new PostsService();
        }

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <param name="search">search.</param>
        /// <param name="onlyWithComments">onlyWithComments.</param>
        /// <param name="onlyWithCommentsInfo">onlyWithCommentsInfo.</param>
        /// <returns>ActionResult</returns>
        // GET: Admin/Posts
        [HttpGet]
        public async Task<ActionResult> Index(string search, bool onlyWithComments = false, bool onlyWithCommentsInfo = false)
        {
            var posts = await _postsService.GetPosts(search, onlyWithComments);
            posts.OnlyWithCommentsInfo = onlyWithComments;
            return View(posts);
        }

        // GET: Posts/Create
        /// <summary>
        /// Create post page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        /// <summary>
        /// Create post action.
        /// </summary>
        /// <param name="postModel">postModel.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(Post postModel)
        {
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return null;
                }
                postModel.CreatedAt = DateTime.Now;
                postModel.Author = User.Identity.GetUserId();
                await _postsService.CreatePost(postModel);
                return RedirectToAction("index", "Posts", new { area = "Admin" });
            }
            catch
            {
                return View();
            }
        }

        // GET: Posts/Edit/5
        /// <summary>
        /// Edit post page.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }
            var postModel = await _postsService.GetPostModel(id.Value);
            return View(postModel.Post);
        }

        // POST: Posts/Edit/5
        /// <summary>
        /// Edit post action.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="editedPost">editedPost.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int? id, Post editedPost)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }
            try
            {
                await _postsService.EditPost(id.Value, editedPost);

                return RedirectToAction("Show/" + id, "Posts", new { area = "" });
            }
            catch (System.Data.DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // POST: Posts/Edit/5
        /// <summary>
        /// Change post status.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="status">status.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(int? id, Status status)
        {
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                await _postsService.ChangePostStatus(id.Value, status);

                return RedirectToAction("Show/" + id, "Posts", new { area = "" });
            }
            catch (System.Data.DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Posts/Delete/5
        /// <summary>
        /// Delete post page.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
        /// <summary>
        /// Delete post action.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="collection">collection.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int? id, FormCollection collection)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                await _postsService.DeletePost(id.Value);

                return RedirectToAction("Index");
            }
            catch
            {
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index");
        }
    }
}