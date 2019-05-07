
using System.Collections;
using Blog.Models;
using Blog.Services.Posts.Interfaces;
using Blog.ViewModels.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

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

        public IList<PostViewModel> GetCurrentUserPosts(string currentUserId)
        {
            BlogContext db = new BlogContext();
            IList<PostViewModel> postModel = new List<PostViewModel>();
            var posts = db.Posts.Where(post => post.Author.Equals(currentUserId)).ToList();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostViewModel post = new PostViewModel();
                post.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(item.Author)).FirstOrDefault();

                ApplicationUser user = userManager.FindByIdAsync(item.Author).Result;
                if (user != null)
                    item.Author = user.UserName;


                post.Post = item;

                post.CommentsCount = db.Comments.Where(comment => comment.PostID.Equals(item.Id)).Count();
                postModel.Add(post);
            });
            return postModel;
        }

        public IList<PostViewModel> GetPosts()
        {
            BlogContext db = new BlogContext();
            IList<PostViewModel> postModel = new List<PostViewModel>();
            var posts = db.Posts.ToList();
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
    }
}