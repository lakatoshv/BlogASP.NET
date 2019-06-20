using Blog.ViewModels.Posts;
using Blog.Core.Dtos;

namespace Blog.Services.Posts.Interfaces
{
    public interface IPostsService
    {
        PostsViewModel GetPosts(SortParametersDto sortParameters, string search);
        PostShowViewModel GetPost(int postId);
        PostShowViewModel GetPostWithComments(int postId, SortParametersDto sortParameters);
        PostsViewModel GetCurrentUserPosts(string currentUserId, SortParametersDto sortParameters, string search);
    }
}
