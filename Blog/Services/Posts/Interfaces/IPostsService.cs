using Blog.Models;
using Blog.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Posts.Interfaces
{
    public interface IPostsService
    {
        IList<PostsViewModel> GetPosts();
        PostViewModel GetPost(int postId);
        IList<PostsViewModel> GetCurrentUserPosts(string currentUserId);
    }
}
