using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Data.Models;
using Blog.Services.Posts;
using Microsoft.AspNet.Identity;

namespace Blog.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {
        /// <summary>
        /// Comments service.
        /// </summary>
        private readonly CommentsService _commentsService;

        /// <summary>
        /// Initializes static members of the <see cref="CommentsController"/> class.
        /// </summary>
        public CommentsController()
        {
            _commentsService = new CommentsService();
        }

        // GET: Admin/Comments
        /// <summary>
        /// Get comments list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> Index()
        {
            return View(await _commentsService.GetAllComments());
        }

        // GET: test/Comments/Details/5
        /// <summary>
        /// Get comments for post.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> PostComments(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(await _commentsService.GetCommentsWithPost(postId.Value));
        }

        // GET: test/Comments/Details/5
        /// <summary>
        /// Get comment by id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = await _commentsService.GetPostWithCommentModel("", id.Value);
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
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var postsWithCommentModel = await _commentsService.GetPostsWithCommentModel("");
            return View(postsWithCommentModel);
        }

        // POST: Admin/Comments/Create
        /// <summary>
        /// Create comment action.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {
            var postsWithCommentModel = await _commentsService.GetPostsWithCommentModel("");
            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;
                comment.Author = User.Identity.GetUserId();
                await _commentsService.CreateComment(comment);
                return RedirectToAction("Index");
            }

            postsWithCommentModel.Comment = comment;
            return View(postsWithCommentModel);
        }

        // POST: Posts/Delete/5
        /// <summary>
        /// Delete comment.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                await _commentsService.DeleteComment(id.Value);

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
