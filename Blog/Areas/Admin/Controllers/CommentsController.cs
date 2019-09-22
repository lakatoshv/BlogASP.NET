﻿using System;
using System.Linq;
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
            return View(_commentsService.GetAllComments().ToList());
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
    }
}
