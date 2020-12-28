using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Models;
using BLog.Data.Repository.Interfaces;
using Blog.Services.Core.Dtos;
using Blog.Services.Posts.Interfaces;
using Blog.Services.GeneralService;

namespace Blog.Services.Posts
{
    /// <summary>
    /// Tags service.
    /// </summary>
    /// <seealso cref="GeneralService{Tag}" />
    /// <seealso cref="ITagsService" />
    public class TagsService : GeneralService<Tag>, ITagsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public TagsService(IRepository<Tag> repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="ITagsService"/>
        public async Task DeletePostTags(int id) =>
            await DeleteAsync(await Where(x => x.PostId == id).ToListAsync());

        /// <inheritdoc cref="ITagsService"/>
        public async Task<List<Tag>> GetPopularTags(SortParametersDto sortParameters)
        {
            var tagsQueryable = GetAll();

            var tagsModel = await tagsQueryable
                .ToListAsync();

            if (sortParameters == null)
            {
                return tagsModel;
            }

            tagsModel = SortTags(tagsModel.AsQueryable(), sortParameters).ToList();

            tagsModel = tagsModel
                .Skip((sortParameters.CurrentPage - 1) * sortParameters.PageSize)
                .Take(sortParameters.PageSize).ToList();

            return tagsModel;
        }

        /// <summary>
        /// Sorts the tags.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>IOrderedQueryable.</returns>
        private static IOrderedQueryable<Tag> SortTags(IQueryable<Tag> tags, SortParametersDto sortParameters)
        {
            if (sortParameters.SortBy.Equals("CreatedAt") && sortParameters.OrderBy.Equals("asc"))
                return tags.OrderBy(x => x.CreatedAt);
            if (sortParameters.SortBy.Equals("CreatedAt") && sortParameters.OrderBy.Equals("desc"))
                return tags.OrderByDescending(x => x.CreatedAt);

            if (sortParameters.SortBy.Equals("Title") && sortParameters.OrderBy.Equals("asc"))
                return tags.OrderBy(x => x.Title);
            if (sortParameters.SortBy.Equals("Title") && sortParameters.OrderBy.Equals("desc"))
                return tags.OrderByDescending(x => x.Title);
            else
                return tags.OrderBy(x => x.Id);
        }
    }
}