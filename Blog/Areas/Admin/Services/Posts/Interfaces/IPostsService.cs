using Blog.Areas.Admin.ViewModels.Posts;
using Blog.Core.Dtos;

namespace Blog.Areas.Admin.Services.Posts.Interfaces
{
    public interface IPostsService
    {
        PostShowViewModel GetPost(int postId);

        PostsViewModel GetCurrentUserPosts(string currentUserId, string search);

        PostsViewModel GetPosts(string search);
    }
}
