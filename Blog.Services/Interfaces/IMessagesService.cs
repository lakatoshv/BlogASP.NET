using System.Threading.Tasks;
using Blog.Data.Models;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Messages service interface.
    /// </summary>
    public interface IMessagesService
    {
        /// <summary>
        /// Inserts the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task Insert(Message message);
    }
}