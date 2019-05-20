
using System.Collections;
using Blog.Models;
using Blog.Services.Posts.Interfaces;
using Blog.ViewModels.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Security.Claims;
using Antlr.Runtime.Misc;
using Blog.Core.Dtos;
using Microsoft.Ajax.Utilities;

namespace Blog.Services.Posts
{
    public class PostsService : IPostsService
    {
        public PostShowViewModel GetPost(int postId)
        {
            BlogContext db = new BlogContext();
            PostShowViewModel postModel = new PostShowViewModel();
            postModel.Post = db.Posts.Where(post => post.Id.Equals(postId)).FirstOrDefault();
            postModel.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(postModel.Post.Author)).FirstOrDefault();
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

            IList<CommentViewModel> commentsViewModel = new List<CommentViewModel>();
            var comments = db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();
            comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                CommentViewModel comm = new CommentViewModel();
                comm.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(comment.Author)).FirstOrDefault();

                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;
                
                comm.comment = comment;

                commentsViewModel.Add(comm);
            });
            postModel.Comments = commentsViewModel;


            return postModel;
        }

        public IList<PostViewModel> GetCurrentUserPosts(string currentUserId, SortParametersDto sortParameters)
        {
            BlogContext db = new BlogContext();
            IList<PostViewModel> postModel = new List<PostViewModel>();
            var posts = this.SortPosts(db.Posts.Where(post => post.Author.Equals(currentUserId)), sortParameters).ToList();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostViewModel post = new PostViewModel();
                post.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(item.Author)).FirstOrDefault();

                ApplicationUser user = userManager.FindByIdAsync(item.Author).Result;
                if (user != null)
                    item.Author = user.UserName;


                post.Post = item;
                post.Post.PostTags = new List<Tag>();

                var tags = db.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToList();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }

                post.CommentsCount = db.Comments.Where(comment => comment.PostID.Equals(item.Id)).Count();
                postModel.Add(post);
            });
            return postModel;
        }

        public IList<PostViewModel> GetPosts(SortParametersDto sortParameters, string search)
        {
            BlogContext db = new BlogContext();
            IList<PostViewModel> postModel = new List<PostViewModel>();

            IEnumerable<Post> postsEnumerable = db.Posts;
            if (!search.IsNullOrWhiteSpace())
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));

            var posts = this.SortPosts(postsEnumerable, sortParameters).ToList();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostViewModel post = new PostViewModel();
                post.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(item.Author)).FirstOrDefault();

                ApplicationUser user = userManager.FindByIdAsync(item.Author).Result;
                if (user != null)
                    item.Author = user.UserName;

                
                post.Post = item;

                var tags = db.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToList();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }
                
                post.CommentsCount = db.Comments.Where(comment => comment.PostID.Equals(item.Id)).Count();
                postModel.Add(post);
            });
            return postModel;
        }

        public IOrderedEnumerable<Post> SortPosts(IEnumerable<Post> posts, SortParametersDto sortParameters)
        {
            BlogContext db = new BlogContext();
            if (sortParameters.SortBy.Equals("CreatedAt") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.CreatedAt);
            if (sortParameters.SortBy.Equals("CreatedAt") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.CreatedAt);

            if (sortParameters.SortBy.Equals("Title") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Title);
            if (sortParameters.SortBy.Equals("Title") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Title);

            if (sortParameters.SortBy.Equals("Author") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Author);
            if (sortParameters.SortBy.Equals("Author") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Author);

            if (sortParameters.SortBy.Equals("Likes") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Likes);
            if (sortParameters.SortBy.Equals("Likes") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Likes);

            if (sortParameters.SortBy.Equals("Dislikes") && sortParameters.OrderBy.Equals("asc"))
                return posts.OrderBy(x => x.Dislikes);
            if (sortParameters.SortBy.Equals("Dislikes") && sortParameters.OrderBy.Equals("desc"))
                return posts.OrderByDescending(x => x.Dislikes);

            else
                return posts.OrderBy(x => x.Id);
            /*
            Expression<Func<Post, object>> sortExpression;
            switch (sortParameters.SortBy)
            {
                case :
                    sortExpression = (x => x.CreatedAt);
                    break;
                case "Title":
                    sortExpression = (x => x.Title);
                    break;
                case "Author":
                    sortExpression = (x => x.Author);
                    break;
                case "Likes":
                    sortExpression = (x => x.Likes);
                    break;
                case "Dislikes":
                    sortExpression = (x => x.Dislikes);
                    break;
                default:
                    sortExpression = (x => x.Id);
                    break;
            }
            */
        }
    }
}