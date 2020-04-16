using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.ViewModels.Users
{
    /// <summary>
    /// Profile view model.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets userData.
        /// </summary>
        public ApplicationUser UserData { get; set; }

        /// <summary>
        /// Gets or sets profileData.
        /// </summary>
        public Profile ProfileData { get; set; }

        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<Post> Posts { get; set; }
    }
}