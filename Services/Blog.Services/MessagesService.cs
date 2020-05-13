using Blog.Data.Models;
using BLog.Data.Repository.Interfaces;
using Blog.Services.Interfaces;
using Blog.Services.GeneralService;

namespace Blog.Services
{
    /// <summary>
    /// Messages service.
    /// </summary>
    /// <seealso>
    ///     <cref>Services.GeneralService{Blog.Data.Models.Message}</cref>
    /// </seealso>
    /// <seealso cref="IMessagesService" />
    public class MessagesService : GeneralService<Message>, IMessagesService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public MessagesService(IRepository<Message> repository)
            : base(repository)
        {
        }
    }
}