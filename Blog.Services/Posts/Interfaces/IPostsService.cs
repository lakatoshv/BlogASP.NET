using System.Threading.Tasks;
using Blog.Core.Enums;
using Blog.Data.Models;
using Blog.Services.Dtos;
using Blog.Services.Dtos.Posts;
using Blog.Services.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Posts service interface.
    /// </summary>
    public interface IPostsService : IGeneralService<Post>
    {
        /// <summary>
        /// Async get sorted and filtered posts by sort parameters.
        /// </summary>
        /// <param name="sortParameters">sortParameters.</param>
        /// <param name="search">search.</param>
        /// <returns>Task.</returns>
        Task<PostsDto> GetPosts(SortParametersDto sortParameters, string search);

        /// <summary>
        /// Async get filtered posts list.
        /// </summary>
        /// <param name="search">search.</param>
        /// <param name="onlyWithComments">onlyWithComments.</param>
        /// <returns>Task.</returns>
        Task<PostsDto> GetPosts(string search, bool onlyWithComments = false);

        /// <summary>
        /// Async get post by id.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>Task.</returns>
        Task<PostShowDto> GetPost(int postId);

        /// <summary>
        /// Async get post model by id.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>Task.</returns>
        Task<PostShowDto> GetPostModel(int postId);

        /// <summary>
        /// Async get post with comments by post id.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <returns>Task.</returns>
        Task<PostShowDto> GetPostWithComments(int postId, SortParametersDto sortParameters);

        /// <summary>
        /// Async get current user posts.
        /// </summary>
        /// <param name="userId">currentUserId.</param>
        /// <param name="search">search.</param>
        /// <returns>Task.</returns>
        Task<PostsDto> GetUserPosts(string userId, string search);

        /// <summary>
        /// Async get sorted and filtered user posts.
        /// </summary>
        /// <param name="userId">currentUserId.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <param name="search">search.</param>
        /// <returns>Task.</returns>
        Task<PostsDto> GetUserPosts(string userId, SortParametersDto sortParameters, string search);

        /// <summary>
        /// Creates the post.
        /// </summary>
        /// <param name="postModel">The post model.</param>
        /// <returns>Task.</returns>
        Task CreatePost(Post postModel);

        /// <summary>
        /// Change post status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task ChangePostStatus(int id, Status status);

    }
}
