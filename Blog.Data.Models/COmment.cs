using System;
using Blog.Data.Core.Models;

namespace Blog.Data.Models
{
    /// <summary>
    /// Comment entity.
    /// </summary>
    public class Comment : BaseDeletableModel<int>
    {
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
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public ApplicationUser Author { get; set; }

        /// <summary>
        /// Gets or sets the comment body.
        /// </summary>
        /// <value>
        /// The comment body.
        /// </value>
        public string CommentBody { get; set; }

        /// <summary>
        /// Gets or sets the likes.
        /// </summary>
        /// <value>
        /// The likes.
        /// </value>
        public int Likes { get; set; } = 0;

        /// <summary>
        /// Gets or sets the dislikes.
        /// </summary>
        /// <value>
        /// The dislikes.
        /// </value>
        public int Dislikes { get; set; } = 0;
    }
}