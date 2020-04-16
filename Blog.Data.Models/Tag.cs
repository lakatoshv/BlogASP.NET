namespace Blog.Data.Models
{
    /// <summary>
    /// Tag model.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets postId.
        /// </summary>
        public int PostId { get; set; }
    }
}