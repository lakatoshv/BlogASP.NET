using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.Interfaces;

namespace Blog.Services
{
    public class MessagesService : IMessagesService
    {
        /// <summary>
        /// Blog context
        /// </summary>
        private readonly BlogContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesService"/> class.
        /// </summary>
        public MessagesService()
        {
            _dbContext = new BlogContext();
        }

        /// <inheritdoc/>
        public async Task Insert(Message message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}