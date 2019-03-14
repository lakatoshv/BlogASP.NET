
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

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.post.Author;
            ApplicationUser user = userManager.FindByIdAsync(author).Result;
            if (user != null)
                postModel.post.Author = user.UserName;

            var comments = db.Comments.Where(comment => comment.PostID.Equals(postId)).ToList();
            comments.ForEach(comment => {
                ApplicationUser commentAuthor = userManager.FindByIdAsync(comment.Author).Result;
                if (commentAuthor != null)
                    comment.Author = commentAuthor.UserName;
            });
            postModel.comments = comments;


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