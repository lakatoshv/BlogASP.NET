using Blog.Data.Models;
using Blog.Data.Models.Repository.Interfaces;
using Blog.Services.Posts.Interfaces;

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
    }
}