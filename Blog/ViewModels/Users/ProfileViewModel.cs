using Blog.Data.Models;

namespace Blog.ViewModels.Users
{
    /// <summary>
    /// Profile view model.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        /// <value>
        /// The user data.
        /// </value>
        public ApplicationUser UserData { get; set; }
    }
}