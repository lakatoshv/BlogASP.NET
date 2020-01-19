using System.Collections.Generic;
using System.Linq;
using Blog.Core.Dtos;
using Blog.Core.HelperClasses;
using Blog.Models;
using Blog.Areas.Admin.Services.Posts.Interfaces;
using Blog.Areas.Admin.ViewModels.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Areas.Admin.Services.Posts
{
    public class CommentsService : ICommentsService
    {
        private readonly BlogContext _db = new BlogContext();
        public CommentWithPostsViewModel GetPostsWithCommentModel(string search)
        {
            CommentWithPostsViewModel postModel = new CommentWithPostsViewModel
            {
                Posts = _db.Posts.ToList()
            };

            return postModel;
        }

        public CommentWithPostViewModel GetPostWithCommentModel(string search, int commentId)
        {
            var comment = _db.Comments.FirstOrDefault(comm => comm.Id.Equals(commentId));
            if (comment == null)
                return null;
            var postModel = new CommentWithPostViewModel
            {
                Post = new PostViewModel { Post = _db.Posts.FirstOrDefault(post => post.Id.Equals(comment.PostID)) },
                Comment = new CommentViewModel { Comment = comment}
            };

            if (postModel.Post != null)
            {
                postModel.Post.Profile =
                    _db.Profiles.FirstOrDefault(profile => profile.ApplicationUser.Equals(postModel.Comment.Comment.Author));
            }
            if (postModel.Comment != null)
            {
                postModel.Comment.Profile = _db.Profiles.FirstOrDefault(profile =>
                    profile.ApplicationUser.Equals(postModel.Comment.Comment.Author));
            }



            return postModel;
        }

        public CommentsViewModel GetAllComments()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            CommentsViewModel commentsViewModel = new CommentsViewModel
            {
                Comments = new List<CommentViewModel>()
            };

            var comments = _db.Comments.ToList();
            comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                CommentViewModel comm = new CommentViewModel
                {
                    Profile = _db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;

                comm.Comment = comment;

                commentsViewModel.Comments.Add(comm);
            });
            return commentsViewModel;
        }

        public IList<Comment> GetCommentsForPost(int postId)
        {
            return _db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();
        }
        public PostShowViewModel GetCommentsWithPost(int postId)
        {
            BlogContext db = new BlogContext();
            PostShowViewModel postModel = new PostShowViewModel
            {
                Post = db.Posts.FirstOrDefault(post => post.Id.Equals(postId))
            };
            postModel.Profile = db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(postModel.Post.Author));
            if (postModel.Post == null) return null;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            ApplicationUser user = userManager.FindByIdAsync(author).Result;
            if (user != null)
                postModel.Post.Author = user.UserName;

            var tags = db.Tags.Where(tag => tag.PostId.Equals(postId)).ToList();
            foreach (var tag in tags)
            {
                postModel.Post.PostTags.Add(tag);
            }

            postModel.Comments = GetCommentsByPostId(postId, author);


            return postModel;
        }
        public CommentsViewModel GetPagedCommentsByPostId(int postId, string author, SortParametersDto sortParameters)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            CommentsViewModel commentsViewModel = new CommentsViewModel
            {
                Comments = new List<CommentViewModel>()
            };
            var comments = _db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();
            comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                CommentViewModel comm = new CommentViewModel
                {
                    Profile = _db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;

                comm.Comment = comment;

                commentsViewModel.Comments.Add(comm);
            });
            commentsViewModel.Comments = commentsViewModel.Comments.Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize).Take(sortParameters.PageSize).ToList();
            commentsViewModel.PageInfo = new PageInfo { PageNumber = sortParameters.CurrentPage, PageSize = sortParameters.PageSize, TotalItems = comments.Count };
            return commentsViewModel;
        }

        public CommentsViewModel GetCommentsByPostId(int postId, string author)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            CommentsViewModel commentsViewModel = new CommentsViewModel
            {
                Comments = new List<CommentViewModel>()
            };
            var comments = _db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();
            comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                CommentViewModel comm = new CommentViewModel
                {
                    Profile = _db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(comment.Author))
                };

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;

                comm.Comment = comment;

                commentsViewModel.Comments.Add(comm);
            });
            return commentsViewModel;
        }

        public void CreateComment(Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
        }
    }
}