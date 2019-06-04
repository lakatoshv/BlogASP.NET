using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Core.Dtos;
using Blog.Core.HelperClasses;
using Blog.Models;
using Blog.Services.Posts.Interfaces;
using Blog.ViewModels.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Services.Posts
{
    public class CommentsService : ICommentsService
    {
        public CommentsViewModel GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters)
        {
            BlogContext db = new BlogContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(author).Result;

            CommentsViewModel commentsViewModel = new CommentsViewModel
            {
                Comments = new List<CommentViewModel>()
            };
            var comments = db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();
            comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                CommentViewModel comm = new CommentViewModel();
                comm.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(comment.Author)).FirstOrDefault();

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;

                comm.comment = comment;

                commentsViewModel.Comments.Add(comm);
            });
            commentsViewModel.Comments = commentsViewModel.Comments.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize).Take(sortParameters.PageSize).ToList();
            commentsViewModel.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = comments.Count };
            return commentsViewModel;
        }
    }
}