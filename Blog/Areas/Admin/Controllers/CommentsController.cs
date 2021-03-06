﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Areas.Admin.ViewModels;
using Blog.Areas.Admin.ViewModels.Posts;
using Blog.Data.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Blog.Services.Posts.Interfaces;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Comments controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class CommentsController : Controller
    {
        /// <summary>
        /// Comments service.
        /// </summary>
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// Posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// The upload from file service.
        /// </summary>
        private readonly IUploadFromFileService _uploadFromFileService;

        /// <summary>
        /// Initializes static members of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="commentsService"></param>
        /// <param name="postsService"></param>
        /// <param name="uploadFromFileService"></param>
        public CommentsController(
            ICommentsService commentsService,
            IPostsService postsService,
            IUploadFromFileService uploadFromFileService)
        {
            _commentsService = commentsService;
            _postsService = postsService;
            _uploadFromFileService = uploadFromFileService;
        }

        // GET: Admin/Comments
        /// <summary>
        /// Get comments list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewBag.Controller = "Comments";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();

            return View(await _commentsService.GetAllComments());
        }
            

        // GET: test/Comments/Details/5
        /// <summary>
        /// Get comments for post.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public async Task<ActionResult> PostComments(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var commentsDto = await _commentsService.GetCommentsForPost(postId.Value, null, null);
            

            return View(new PostViewModel
            {
                Post = await _postsService.FindAsync(postId),
                Comments = new CommentsViewModel
                {
                    Comments = commentsDto.Comments,
                    DisplayType = commentsDto.DisplayType,
                    PageInfo = commentsDto.PageInfo,
                }
            });
        }

        // GET: test/Comments/Details/5
        /// <summary>
        /// Get comment by id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = await _commentsService.GetComment(id.Value);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Admin/Comments/Create
        /// <summary>
        /// Create comment page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PostId = _postsService.GetPostsSelectList(null);
            return View();
        }

        // POST: Admin/Comments/Create
        /// <summary>
        /// Create comment action.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {
            ViewBag.PostId = _postsService.GetPostsSelectList(null);
            if (!ModelState.IsValid)
            {
                return View();
            }

            comment.CreatedAt = DateTime.Now;
            comment.AuthorId = User.Identity.GetUserId();
            await _commentsService.InsertAsync(comment);
            return RedirectToAction("Index");

        }

        // POST: Posts/Delete/5
        /// <summary>
        /// Delete comment.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                await _commentsService.DeleteAsync(id.Value);

                return RedirectToAction("Index");
            }
            catch
            {
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
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
                .UploadCommentsFromExcel(uploadFile?.ExcelFile.InputStream, currentUserId).ConfigureAwait(false);

            ViewBag.Controller = "Posts";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, result.ExceptionMessage);

            return View("Index");
        }
    }
}
