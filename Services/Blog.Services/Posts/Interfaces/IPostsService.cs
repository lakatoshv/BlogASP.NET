using System.Threading.Tasks;
using Blog.Core.Enums;
using Blog.Data.Models;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Posts service interface.
    /// </summary>
    public interface IPostsService : IGeneralService<Post>
    {
        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <param name="search">The search.</param>
        /// <param name="onlyWithComments">if set to <c>true</c> [only with comments].</param>
        /// <returns></returns>
        Task<PostsDto> GetPosts(SortParametersDto sortParameters, string search, bool onlyWithComments = false);

        /// <summary>
        /// Async get sorted and filtered user posts.
        /// </summary>
        /// <param name="userId">currentUserId.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <param name="search">search.</param>
        /// <returns>Task.</returns>
        Task<PostsDto> GetUserPosts(string userId, SortParametersDto sortParameters, string search);

        /// <summary>
        /// Async get post by id.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <returns>Task.</returns>
        Task<PostShowDto> GetPost(int postId, SortParametersDto sortParameters);

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