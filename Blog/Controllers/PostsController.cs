using Microsoft.AspNet.Identity;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Core.Attributes;
using Blog.Data.Models;
using Blog.Services.Core.Dtos;
using Blog.Services.Posts.Interfaces;
using Microsoft.Ajax.Utilities;

namespace Blog.Controllers
{
    /// <summary>
    /// Posts controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public class PostsController : Controller
    {
        /// <summary>
        /// The posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// The comments service.
        /// </summary>
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// The tags service.
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="postsService">The posts service.</param>
        /// <param name="commentsService"></param>
        /// <param name="tagsService"></param>
        public PostsController(
            IPostsService postsService,
            ICommentsService commentsService,
            ITagsService tagsService)
        {
            _postsService = postsService;
            _commentsService = commentsService;
            _tagsService = tagsService;
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
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var sortParameters = new SortParametersDto()
            {
                CurrentPage = page,
                PageSize = 10
            };

            var postModelToUpdate = await _postsService.FindAsync(id.Value);
            if (postModelToUpdate != null && page == 1)
            {
                postModelToUpdate.Seen++;
                await _postsService.UpdateAsync(postModelToUpdate);
            }

            var postModel = await _postsService.GetPost(id.Value, sortParameters);
            if(postModel == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            return View(postModel);
        }

        /// <summary>
        /// Get Current user posts.
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
        public ActionResult Create() => View();

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
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                // TODO remove from here
                postModel.PostTags = new Collection<Tag>();
                //postModel.CreatedAt = DateTime.Now;
                postModel.AuthorId = User.Identity.GetUserId();

                if (!postModel.Tags.IsNullOrWhiteSpace())
                {
                    var tags = postModel.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tags)
                    {
                        postModel.PostTags.Add(new Tag()
                        {
                            Title = tag,
                        });
                    }
                }

                await _postsService.InsertAsync(postModel);
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
        public async Task<ActionResult> Like(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                var post = await _postsService.FindAsync(id);
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
            if (id == null) return RedirectToAction("Index", "Posts");
            try
            {
                var post = await _postsService.FindAsync(id);
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

            var postModel = await _postsService.FindAsync(id.Value);
            
            return View(postModel);
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Post editedPost)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            try
            {
                var postModel = await _postsService.FindAsync(id.Value);

                editedPost.AuthorId = postModel.AuthorId;
                editedPost.Likes = postModel.Likes;
                editedPost.Dislikes = postModel.Dislikes;
                editedPost.Seen = postModel.Seen;
                editedPost.CreatedAt = DateTime.Now;

                postModel.Title = editedPost.Title;
                postModel.Description = editedPost.Description;
                postModel.Content = editedPost.Content;
                postModel.ImageUrl = editedPost.ImageUrl;
                postModel.Tags = editedPost.Tags;
                await _postsService.UpdateAsync(postModel);
                return RedirectToAction("Show/" + id, "Posts");
            }
            catch (DataException dex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", dex.Message);

                return RedirectToAction("Index", "Posts");
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
        public ActionResult Delete(int id) => View();

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
                await _commentsService.DeletePostComments(id.Value);
                await _tagsService.DeletePostTags(id.Value);
                await _postsService.DeleteAsync(id.Value);

                return RedirectToAction("Index");
            }
            catch(Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Releases unmanaged and, if indicated, managed resources.
        /// </summary>
        /// <param name="disposing">True to free all resources (managed and unmanaged); false to free only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _postsService.Dispose();
            base.Dispose(disposing);
        }
    }
}
