using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Areas.Admin.ViewModels.Users
{
    /// <summary>
    /// Profile view model.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets the profile data.
        /// </summary>
        /// <value>
        /// The profile data.
        /// </value>
        public Profile ProfileData { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public IList<Post> Posts { get; set; }
    }
}