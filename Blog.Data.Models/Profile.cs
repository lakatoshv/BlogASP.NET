using System.ComponentModel.DataAnnotations.Schema;
using Blog.Data.Core;
using Blog.Data.Core.Models;

namespace Blog.Data.Models
{
    /// <summary>
    /// Profile entity.
    /// </summary>
    public class Profile : BaseDeletableModel<int>
    {
        /// <summary>
        /// Gets or sets the application user.
        /// </summary>
        /// <value>
        /// The application user.
        /// </value>
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Gets or sets the application user.
        /// </summary>
        /// <value>
        /// The application user.
        /// </value>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Gets or sets the profile img.
        /// </summary>
        /// <value>
        /// The profile img.
        /// </value>
        public string ProfileImage { get; set; }
    }
}