using System;

namespace Blog.Data.Models
{
    /// <summary>
    /// Comment model.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets postID.
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Gets or sets author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets commentBody.
        /// </summary>
        public string CommentBody { get; set; }

        /// <summary>
        /// Gets or sets createdAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets likes.
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Gets or sets dislikes.
        /// </summary>
        public int Dislikes { get; set; }
    }
}