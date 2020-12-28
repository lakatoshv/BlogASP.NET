using System.Threading.Tasks;
using Blog.Services.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace Blog.Services.Identity
{
    /// <summary>
    /// Email service.
    /// </summary>
    /// <seealso cref="IIdentityMessageService" />
    public class EmailService : IEmailService, IIdentityMessageService
    {
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        public Task SendAsync(IdentityMessage message) =>
            Task.FromResult(0);
    }
}