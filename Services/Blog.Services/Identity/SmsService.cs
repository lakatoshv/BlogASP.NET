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
        /// <returns>Task.</returns>
        public Task SendAsync(IdentityMessage message) =>
            Task.FromResult(0);
    }
}