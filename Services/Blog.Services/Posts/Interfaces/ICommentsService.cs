using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Comments service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{T}" />
    public interface ICommentsService : IGeneralService<Comment>
    {
        /// <summary>
        /// Async get list of all posts.
        /// </summary>
        /// <returns>CommentsDto.</returns>
        Task<CommentsDto> GetAllComments();

        /// <summary>
        /// Gets the comments for post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <param name="authorId">The author identifier.</param>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>Task.</returns>
        Task<CommentsDto> GetCommentsForPost(int? postId, string authorId,
            SortParametersDto sortParameters);

        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task<Comment> GetComment(int id);

        /// <summary>
        /// Async delete post comments.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task.</returns>
        Task DeletePostComments(int id);
    }
}