using Blog.Services.Posts.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Attributes;
using Blog.Data.Models;
using Blog.Services.Dtos;

namespace Blog.Controllers
{
    /// <summary>
    /// Posts controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class PostsController : Controller
    {
        /// <summary>
        /// The posts service
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        // GET: Posts        
        /// <summary>
        /// Indexes the specified search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="page">The page.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Index(string search, string sortBy, string orderBy, int page = 1)
        {
            var sortParameters = new SortParametersDto()
            {
                OrderBy = orderBy ?? "asc",
                SortBy = sortBy ?? "Title",
                CurrentPage = page,
                PageSize = 10
            };
            var posts = await _postsService.GetPosts(sortParameters, search);
            return View(posts);
        }

        // GET: Posts/Show/5        
        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sorts">The sorts.</param>
        /// <param name="page">The page.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Show(int? id, string sorts, int page = 1)
        {
            if (id == null) return RedirectToAction("Index", "Posts");

            var sortParameters = new SortParametersDto()
            {
                CurrentPage = page,
                PageSize = 10
            };

            var postModelToUpdate =  _postsService.Find(id.Value);
            if (postModelToUpdate != null && page == 1)
            {
                postModelToUpdate.Seen++;
                await _postsService.UpdateAsync(postModelToUpdate);
            }

            var postModel = await _postsService.GetPostWithComments(id.Value, sortParameters);
            if(postModel == null) return RedirectToAction("Index", "Posts");

            return View(postModel);
        }

        /// <summary>
        /// Current user posts.
        /// </summary>
        /// <param name="display">The display.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="search">The search.</param>
        /// <param name="page">The page.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> MyPosts(string display, string sortBy, string orderBy, string search, int page = 1)
        {
            var sortParameters = new SortParametersDto()
            {
                OrderBy = orderBy ?? "asc",
                SortBy = sortBy ?? "Title",
                CurrentPage = page,
                PageSize = 10,
                DisplayType = display ?? "list"
            };

            var posts = await _postsService.GetUserPosts(User.Identity.GetUserId(), sortParameters, search);
            posts.DisplayType = display ?? "list";

            return View(posts);
        }

        // GET: Posts/Create        
        /// <summary>
        /// Creates this instance.
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
        /// Creates the specified post model.
        /// </summary>
        /// <param name="postModel">The post model.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(Post postModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                //post.CreatedAt = DateTime.Now;
                postModel.CreatedAt = DateTime.Now;
                postModel.Author = User.Identity.GetUserId();
                await _postsService.CreatePost(postModel);
                return RedirectToAction("index", "Posts");
            }
            catch
            {
                return View();
            }
        }

        // GET: Like/5        
        /// <summary>
        /// Likes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult>  Like(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                var post = _postsService.Find(id.Value);
                if (post == null)
                {
                    return RedirectToAction("Show/" + id, "Posts");
                }

                post.Likes++;
                await _postsService.UpdateAsync(post);

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Dislike/5        
        /// <summary>
        /// Dislikes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Dislike(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                var post = _postsService.Find(id.Value);
                if (post == null)
                {
                    return RedirectToAction("Show/" + id, "Posts");
                }

                post.Dislikes++;
                await _postsService.UpdateAsync(post);

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Posts/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        [Authorize]
        [CheckPermissionsToEditForPosts]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var postModel = await _postsService.GetPost(id.Value);
            return View(postModel.Post);
        }

        // POST: Posts/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="editedPost">The edited post.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [Authorize]
        [CheckPermissionsToEditForPosts]
        public async Task<ActionResult> Edit(int? id, Post editedPost)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                var postModel = await _postsService.GetPost(id.Value);
                if (!postModel.Post.Author.Equals(User.Identity.GetUserId()))
                {
                    return RedirectToAction("Show/" + postModel.Post.Id, "Posts");
                }


                // post.Author = postModel.Post.Author;
                // editedPost.Likes = postModel.Post.Likes;
                // editedPost.Dislikes = postModel.Post.Dislikes;
                // editedPost.Seen = postModel.Post.Seen;
                // editedPost.CreatedAt = DateTime.Now;
                await _postsService.UpdateAsync(editedPost);

                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException /* dex */)
            {
                return RedirectToAction("Index", "Posts");
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        // GET: Posts/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Authorize]
        [CheckPermissionsToEditForPosts]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [Authorize]
        [CheckPermissionsToEditForPosts]
        public async Task<ActionResult> Delete(int? id, FormCollection collection)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var post = await _postsService.FindAsync(id.Value);
                if (post != null && !post.Author.Equals(User.Identity.GetUserId()))
                {
                    return RedirectToAction("Index", "Posts");
                }

                await _postsService.DeleteAsync(id.Value);

                return RedirectToAction("Index");
            }
            catch
            {
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _postsService.Dispose();
            base.Dispose(disposing);
        }
    }
}
