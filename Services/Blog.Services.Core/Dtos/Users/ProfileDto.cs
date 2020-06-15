using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Services.Core.Dtos.Users
{
    /// <summary>
    /// Profile dto.
    /// </summary>
    public class ProfileDto
    {
        /// <summary>
        /// Gets or sets profileData.
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<Post> Posts { get; set; }
    }
}