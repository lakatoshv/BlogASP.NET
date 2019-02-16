using Blog.Models;
using Blog.Services.Posts.Interfaces;
using Blog.ViewModels.Posts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace Blog.Services.Posts
{
    public class PostsService : IPostsService
    {
        public PostViewModel GetPost(int postId)
        {
            BlogContext db = new BlogContext();
            PostViewModel postModel = new PostViewModel();
            postModel.post = db.Posts.Where(post => post.Id.Equals(postId)).FirstOrDefault();
            postModel.comments = db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(postModel.post.Author).Result;
            postModel.post.Author = user.UserName;

            postModel.comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                if (user != null)
                    comment.Author = commentAuthor.UserName;
            });

            return postModel;
        }

        public IList<Post> GetPosts()
        {
            BlogContext db = new BlogContext();
            var posts = db.Posts.ToList();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                ApplicationUser user = userManager.FindByIdAsync(item.Author).Result;
                if (user != null)
                    item.Author = user.UserName;
            });
            return posts;
        }
    }
}