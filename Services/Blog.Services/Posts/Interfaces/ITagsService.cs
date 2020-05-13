using Blog.Data.Models;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Tags service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{T}" />
    public interface ITagsService : IGeneralService<Tag>
    {
    }
}