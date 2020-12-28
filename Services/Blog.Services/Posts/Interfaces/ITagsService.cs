using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Core.Dtos;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Tags service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{T}" />
    public interface ITagsService : IGeneralService<Tag>
    {
        /// <summary>
        /// Deletes the post tags.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task DeletePostTags(int id);

        /// <summary>
        /// Gets the popular tags.
        /// </summary>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>Task.</returns>
        Task<List<Tag>> GetPopularTags(SortParametersDto sortParameters);
    }
}