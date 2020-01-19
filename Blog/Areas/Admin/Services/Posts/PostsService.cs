using Blog.Models;
using Blog.Areas.Admin.Services.Posts.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Blog.Areas.Admin.ViewModels.Posts;
using Microsoft.Ajax.Utilities;

namespace Blog.Areas.Admin.Services.Posts
{
    public class PostsService : IPostsService
    {
        public Post GePost(int postId)
        {
            BlogContext db = new BlogContext();
            return db.Posts.FirstOrDefault(post => post.Id.Equals(postId));
        }
        public PostShowViewModel GetPostModel(int postId)
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
            
            return postModel;
        }

        public PostsViewModel GetCurrentUserPosts(string currentUserId, string search)
        {
            BlogContext db = new BlogContext();
            IList<PostViewModel> postModel = new List<PostViewModel>();

            IEnumerable<Post> postsEnumerable = db.Posts.Where(post => post.Author.Equals(currentUserId));
            if (!search.IsNullOrWhiteSpace())
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));

            var posts =postsEnumerable.ToList();

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostViewModel post = new PostViewModel
                {
                    Profile = db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(item.Author))
                };

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

                post.CommentsCount = db.Comments.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });

            return new PostsViewModel { Posts = postModel };
        }

        public PostsViewModel GetPosts(string search, bool onlyWithComments = false)
        {
            BlogContext db = new BlogContext();
            IList<PostViewModel> postModel = new List<PostViewModel>();

            IEnumerable<Post> postsEnumerable = db.Posts;
            if (!search.IsNullOrWhiteSpace())
                postsEnumerable = postsEnumerable.Where(post => post.Title.Equals(search));

            var posts =postsEnumerable.ToList();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            posts.ForEach(item => {
                PostViewModel post = new PostViewModel
                {
                    Profile = db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(item.Author)),
                    CommentsCount = db.Comments.Count(comment => comment.PostID.Equals(item.Id))
                };
                ApplicationUser user = userManager.FindByIdAsync(item.Author).Result;
                if (user != null)
                    item.Author = user.UserName;

                
                post.Post = item;

                var tags = db.Tags.Where(tag => tag.PostId.Equals(item.Id)).ToList();
                foreach (var tag in tags)
                {
                    post.Post.PostTags.Add(tag);
                }
                
                post.CommentsCount = db.Comments.Count(comment => comment.PostID.Equals(item.Id));
                postModel.Add(post);
            });

            if (onlyWithComments)
                postModel = postModel.Where(x => x.CommentsCount > 0).ToList();
            return new PostsViewModel { Posts = postModel };
        }
    }
}