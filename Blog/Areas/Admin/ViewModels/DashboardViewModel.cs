using System.Collections.Generic;
using Blog.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Areas.Admin.ViewModels
{
    /// <summary>
    /// Dashboard view model.
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IList<ApplicationUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public IList<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IList<IdentityRole> Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardViewModel"/> class.
        /// </summary>
        public DashboardViewModel()
        {
            Users = new List<ApplicationUser>();
            Posts = new List<Post>();
            Tags = new List<Tag>();
            Roles = new List<IdentityRole>();
        }
    }
}