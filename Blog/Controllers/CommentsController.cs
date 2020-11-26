using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Core.Attributes;
using Blog.Data.Models;
using Blog.Services.Posts.Interfaces;

namespace Blog.Controllers
{
    /// <summary>
    /// Comments controller.
    /// </summary>
    [Authorize]
    public class CommentsController : Controller
    {
        /// <summary>
        /// The comments service.
        /// </summary>
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="commentsService">The comments service.</param>
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        // GET: Comments/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckPermissionsToEditForComments]
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Comments/Create        
        /// <summary>
        /// Creates the specified comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                comment.AuthorId = User.Identity.GetUserId();
                if (!ModelState.IsValid)
                {
                    return View(comment);
                }

                await _commentsService.InsertAsync(comment);

                return RedirectToAction("Show/" + comment.PostId, "Posts");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comments/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [CheckPermissionsToEditForComments]
        public async Task<ActionResult> Edit(int id)
        {
            var comment = await _commentsService.Where(x => x.Id == id)
                .Include(x => x.Post)
                .Include(x => x.Author)
                .Include(x => x.Author.Profile)
                .FirstOrDefaultAsync();

            return View(comment);
        }

        // POST: Comments/Edit/5        
        /// <summary>
        /// Edits the specified comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CheckPermissionsToEditForComments]
        public async Task<ActionResult> Edit(Comment comment)
        {
            try
            {
                var originalComment = await _commentsService.FindAsync(comment.Id);
                if (originalComment == null)
                {
                    return RedirectToAction("Show/" + comment.PostId, "Posts");
                }

                if (!originalComment.AuthorId.Equals(User.Identity.GetUserId()))
                {
                    return RedirectToAction("Show/" + comment.PostId, "Posts");
                }

                
                originalComment.CommentBody = comment.CommentBody;
                await _commentsService.UpdateAsync(originalComment);

                return RedirectToAction("Show/" + comment.PostId, "Posts");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(comment);
            
            
        }

        // POST: Comments/Delete/5
        /// <summary>
        /// Delete post.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermissionsToEditForComments]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var comment = await _commentsService.FindAsync(id.Value);
                if (comment != null && !comment.AuthorId.Equals(User.Identity.GetUserId()))
                {
                    return RedirectToAction("Show/" + comment.PostId, "Posts");
                }

                if (comment != null)
                {
                    await _commentsService.DeleteAsync(comment);
                }

                if (comment != null)
                {
                    return RedirectToAction("Show/" + comment.PostId, "Posts");
                }
            }
            catch
            {
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return RedirectToAction("Index", "Posts");
        }

        /// <summary>
        /// Releases unmanaged and, if indicated, managed resources.
        /// </summary>
        /// <param name="disposing">True to free all resources (managed and unmanaged); false to free only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _commentsService.Dispose();
            base.Dispose(disposing);
        }
    }
}
