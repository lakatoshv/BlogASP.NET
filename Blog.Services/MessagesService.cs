using Blog.Data.Models;
using Blog.Data.Models.Repository.Interfaces;
using Blog.Services.Interfaces;

namespace Blog.Services
{
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