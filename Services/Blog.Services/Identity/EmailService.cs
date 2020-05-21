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
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            // TODO Connect email service here to send an email message.
            return Task.FromResult(0);
        }
    }
}