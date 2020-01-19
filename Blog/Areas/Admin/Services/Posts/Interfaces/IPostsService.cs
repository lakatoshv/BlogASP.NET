using Blog.Areas.Admin.ViewModels.Posts;
using Blog.Core.Dtos;
using Blog.Models;

namespace Blog.Areas.Admin.Services.Posts.Interfaces
{
    public interface IPostsService
    {
        Post GePost(int postId);
        PostShowViewModel GetPostModel(int postId);

        PostsViewModel GetCurrentUserPosts(string currentUserId, string search);

        PostsViewModel GetPosts(string search, bool onlyWithComments = false);
    }
}
