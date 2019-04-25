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
        IList<PostViewModel> GetPosts();
        PostShowViewModel GetPost(int postId);
        IList<PostViewModel> GetCurrentUserPosts(string currentUserId);
    }
}
