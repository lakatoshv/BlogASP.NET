using Blog.Data.Models;
using Blog.Services.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Tags service interface.
    /// </summary>
    /// <seealso cref="Blog.Services.Interfaces.IGeneralService{Tag}" />
    public interface ITagsService : IGeneralService<Tag>
    {
    }
}