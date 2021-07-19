using System.IO;
using System.Threading.Tasks;
using Blog.Services.Core.Dtos;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Upload from file service interface.
    /// </summary>
    public interface IUploadFromFileService
    {
        /// <summary>
        /// Uploads the posts from excel.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>Task.</returns>
        Task<ResultDto> UploadPostsFromExcel(Stream inputStream, string currentUserId);

        /// <summary>
        /// Uploads the comments from excel.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>Task.</returns>
        Task<ResultDto> UploadCommentsFromExcel(Stream inputStream, string currentUserId);

        /// <summary>
        /// Uploads the roles from excel.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>Task.</returns>
        Task<ResultDto> UploadRolesFromExcel(Stream inputStream);
    }
}