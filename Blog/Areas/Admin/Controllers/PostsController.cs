using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Core.Enums;
using Microsoft.AspNet.Identity;
using Blog.Data.Models;
using Blog.Services.Posts.Interfaces;
using System.Data;
using System.IO;
using System.Linq;
using Blog.Areas.Admin.ViewModels;
using Blog.Services.Interfaces;
using ClosedXML.Excel;

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
        /// The comments service.
        /// </summary>
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// The tags service.
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// The upload from file service.
        /// </summary>
        private readonly IUploadFromFileService _uploadFromFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="postsService">The posts service.</param>
        /// <param name="commentsService"></param>
        /// <param name="tagsService"></param>
        /// <param name="uploadFromFileService"></param>
        public PostsController(IPostsService postsService, 
            ICommentsService commentsService, 
            ITagsService tagsService, 
            IUploadFromFileService uploadFromFileService)
        {
            _postsService = postsService;
            _commentsService = commentsService;
            _tagsService = tagsService;
            _uploadFromFileService = uploadFromFileService;
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
            ViewBag.Controller = "Posts";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();

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
        public ActionResult Create() => View();

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
            catch (DataException dex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", dex.Message);

                return RedirectToAction("Index", "Posts");
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
        /// Uploads the items from excel.
        /// </summary>
        /// <param name="uploadFile">The upload file.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadItemsFromExcel(UploadFileViewModel uploadFile)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var currentUserId = User.Identity.GetUserId();
            var result = await _uploadFromFileService
                .UploadPostsFromExcel(uploadFile?.ExcelFile.InputStream, currentUserId).ConfigureAwait(false);

            ViewBag.Controller = "Posts";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, result.ExceptionMessage);

            return View("Index");
        }

        /// <summary>
        /// Exports to excel.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> ExportToExcel()
        {
            try
            {
                var posts = await _postsService.GetAllAsync().ConfigureAwait(false);
                var dataTable = new DataTable("Posts");
                // Headers.
                dataTable.Columns.AddRange(
                    new[]
                    {
                        new DataColumn("Title"),
                        new DataColumn("Description"),
                        new DataColumn("Content"),
                        new DataColumn("Author"),
                        new DataColumn("Seen"),
                        new DataColumn("Likes"),
                        new DataColumn("Dislikes"),
                        new DataColumn("ImageUrl"),
                        new DataColumn("Status"),
                        new DataColumn("Tags"),
                        new DataColumn("Comments count")
                    });

                // Rows.
                foreach (var post in posts)
                {
                    var tags = "";
                    post.PostTags.Select(x => tags = string.IsNullOrEmpty(tags) ? x.Title : $"{tags}, {x.Title}");
                    dataTable.Rows.Add(
                        post.Title,
                        post.Description,
                        post.Content,
                        $"{post.Author.FirstName} {post.Author.LastName}({post.Author.Email})",
                        post.Seen,
                        post.Likes,
                        post.Dislikes,
                        post.ImageUrl,
                        post.Status.ToString(),
                        tags,
                        post.Comments.Count
                    );
                }

                using (var wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dataTable);

                    using (var stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        return File(stream.ToArray(),
                            "application/wnd.openxmlformats-officedocument.spreadsheetml.sheet", "Posts.xlsx");
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}