using Blog.Data.Core.Models;

namespace Blog.Data.Models
{
    /// <summary>
    /// Tag entity
    /// </summary>
    /// <seealso>
    ///     <cref>BaseDeletableModel{int}</cref>
    /// </seealso>
    public class Tag : BaseDeletableModel<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>
        /// The post.
        /// </value>
        public virtual Post Post { get; set; }
    }
}