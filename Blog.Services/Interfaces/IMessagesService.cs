using Blog.Data.Models;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Messages service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Message}" />
    public interface IMessagesService : IGeneralService<Message>
    {
    }
}