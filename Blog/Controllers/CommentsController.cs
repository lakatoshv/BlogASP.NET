using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Attributes;
using Blog.Data.Models;
using Blog.Services.Posts;

namespace Blog.Controllers
{
    /// <summary>
    /// Comments controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize]
    public class CommentsController : Controller
    {
        /// <summary>
        /// The comments service
        /// </summary>
        private readonly CommentsService _commentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        public CommentsController()
        {
            _commentsService = new CommentsService();
        }

        // GET: Comments/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
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
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermissionsToEditForComments]
        public async Task<ActionResult> Create(Comment comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            comment.CreatedAt = DateTime.Now;
            comment.Author = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                

                await _commentsService.CreateComment(comment);
                return RedirectToAction("Show/" + comment.PostID, "Posts");
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
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [CheckPermissionsToEditForComments]
        public ActionResult Edit(int? id)
        {
            if(!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermissionsToEditForComments]
        public async Task<ActionResult> Edit(Comment comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var originalComment = await _commentsService.GetComment(comment.Id);
                if (originalComment != null && !originalComment.Author.Equals(User.Identity.GetUserId()))
                {
                    return RedirectToAction("Show/" + comment.PostID, "Posts");
                }

                comment.Author = User.Identity.GetUserId();
                comment.CreatedAt = DateTime.Now;

                await _commentsService.Update(comment);

                return RedirectToAction("Show/" + comment.PostID, "Posts");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(comment);
            
            
        }

        /*
        // GET: Comments/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            return View();
        }
        */

        // POST: Comments/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [CheckPermissionsToEditForComments]
        public async Task<ActionResult>  Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var comment = await _commentsService.GetComment(id.Value);
                if (comment != null && !comment.Author.Equals(User.Identity.GetUserId()))
                {
                    return RedirectToAction("Show/" + comment.PostID, "Posts");
                }

                if (comment != null)
                {
                    await _commentsService.DeleteComment(id.Value);
                }

                if (comment != null) return RedirectToAction("Show/" + comment.PostID, "Posts");
            }
            catch
            {
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index", "Posts");
        }
    }
}
