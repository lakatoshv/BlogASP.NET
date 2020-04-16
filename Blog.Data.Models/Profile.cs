namespace Blog.Data.Models
{
    /// <summary>
    /// Profile model.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets applicationUser.
        /// </summary>
        public string ApplicationUser { get; set; }

        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets profileImg.
        /// </summary>
        public string ProfileImg { get; set; }
    }
}