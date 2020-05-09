using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Core;
using Blog.Data.Core.Models.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Data.Models
{
    /// <summary>
    /// Application user entity
    /// </summary>
    /// <seealso cref="IdentityUser" />
    /// <seealso cref="IAuditInfo" />
    /// <seealso cref="IDeletableEntity" />
    /// <seealso>
    ///     <cref>IEntity{string}</cref>
    /// </seealso>
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity, IEntity<string>
    {
        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType must match the type defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            /*var db = new BlogContext();
            var profile = db.Profiles.FirstOrDefault(pr => pr.ApplicationUser.Equals(Id));
            if (profile != null)*/
            userIdentity.AddClaims(new[]
            {
                new Claim("FirstName", !string.IsNullOrWhiteSpace(FirstName) ? FirstName : ""),
                new Claim("LastName", !string.IsNullOrWhiteSpace(LastName) ? LastName : ""),
                // new Claim("ProfileImg", !string.IsNullOrWhiteSpace(profile.ProfileImg) ? profiProfileImg : ""),
                // new Claim("ProfileId", profile.Id.ToString())
            });

            return userIdentity;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <inheritdoc/>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <inheritdoc/>
        public DateTime? ModifiedOn { get; set; }

        /// <inheritdoc/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        /// <value>
        /// The profile identifier.
        /// </value>
        [ForeignKey("Profile")]
        public int ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public virtual Profile Profile { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public virtual ICollection<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public virtual ICollection<Message> Messages { get; set; }
    }
}