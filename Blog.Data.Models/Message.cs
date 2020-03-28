namespace Blog.Data.Models
{
    /// <summary>
    /// Message model.
    /// </summary>
    public class Message
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
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets body.
        /// </summary>
        public string Body { get; set; }
    }
}