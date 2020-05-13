using Blog.Data.Models;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Messages service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{T}" />
    public interface IMessagesService : IGeneralService<Message>
    {
    }
}