using System.Threading.Tasks;
using Blog.Services.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace Blog.Services.Identity
{
    /// <summary>
    /// Sms service.
    /// </summary>
    /// <seealso cref="IIdentityMessageService" />
    public class SmsService : ISmsService, IIdentityMessageService
    {
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            // TODO Connect the SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}