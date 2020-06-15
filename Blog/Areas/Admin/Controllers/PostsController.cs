using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Core.Enums;
using Microsoft.AspNet.Identity;
using Blog.Data.Models;
using Blog.Services.Posts.Interfaces;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Posts controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class PostsController : Controller
    {
        /// <summary>
        /// Posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="postsService">The posts service.</param>
        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
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
            var posts = await _postsService.GetPosts(null, search, onlyWithComments);
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
                postModel.AuthorId = User.Identity.GetUserId();
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

            return View(await _postsService.FindAsync(id.Value));
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
                await _postsService.UpdateAsync(editedPost);

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
                await _postsService.DeleteAsync(id.Value);

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