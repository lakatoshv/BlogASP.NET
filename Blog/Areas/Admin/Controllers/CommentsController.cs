using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Blog.Areas.Admin.Services.Posts;
using Blog.Models;
using Microsoft.AspNet.Identity;

namespace Blog.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {
        private readonly CommentsService _commentsService = new CommentsService();

        // GET: Admin/Comments
        public ActionResult Index()
        {
            return View(_commentsService.GetAllComments());
        }

        // GET: test/Comments/Details/5
        public ActionResult PostComments(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(_commentsService.GetCommentsWithPost(postId.Value));
        }

        // GET: test/Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = _commentsService.GetPostWithCommentModel("", id.Value);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Admin/Comments/Create
        public ActionResult Create()
        {
            var postsWithCommentModel = _commentsService.GetPostsWithCommentModel("");
            return View(postsWithCommentModel);
        }

        // POST: Admin/Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            var postsWithCommentModel = _commentsService.GetPostsWithCommentModel("");
            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;
                comment.Author = User.Identity.GetUserId();
                _commentsService.CreateComment(comment);
                return RedirectToAction("Index");
            }

            postsWithCommentModel.Comment = comment;
            return View(postsWithCommentModel);
        }

        // GET: Posts/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                BlogContext db = new BlogContext();
                var commentForDelete = db.Comments.FirstOrDefault(comment => comment.Id.Equals(id.Value));

                if (commentForDelete != null)
                {
                    db.Comments.Remove(commentForDelete);
                    db.SaveChanges();
                }

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
