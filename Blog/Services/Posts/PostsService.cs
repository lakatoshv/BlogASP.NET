
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
        public PostViewModel GetPost(int postId)
        {
            BlogContext db = new BlogContext();
            PostViewModel postModel = new PostViewModel();
            postModel.post = db.Posts.Where(post => post.Id.Equals(postId)).FirstOrDefault();
            postModel.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(postModel.post.Author)).FirstOrDefault();
            if (postModel.post == null) return null;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.post.Author;
            ApplicationUser user = userManager.FindByIdAsync(author).Result;
            if (user != null)
                postModel.post.Author = user.UserName;
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
            postModel.comments = commentsViewModel;


            return postModel;
        }

        public IList<PostsViewModel> GetCurrentUserPosts(string currentUserId)
        {
            BlogContext db = new BlogContext();
            IList<PostsViewModel> postModel = new List<PostsViewModel>();
            var posts = db.Posts.Where(post => post.Author.Equals(currentUserId)).ToList();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostsViewModel post = new PostsViewModel();
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

        public IList<PostsViewModel> GetPosts()
        {
            BlogContext db = new BlogContext();
            IList<PostsViewModel> postModel = new List<PostsViewModel>();
            var posts = db.Posts.ToList();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostsViewModel post = new PostsViewModel();
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
    }
}