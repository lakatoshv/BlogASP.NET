using Blog.Core.Enums;
using Blog.Data.Models;

namespace Blog.Services.Dtos.Posts
{
    /// <summary>
    /// Post show dto.
    /// </summary>
    public class PostShowDto
    {
        /// <summary>
        /// Gets or sets post.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// Gets or sets comments.
        /// </summary>
        public CommentsDto Comments { get; set; }

        /// <summary>
        /// Gets or sets commentsCount.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets profile.
        /// </summary>
        public Profile Profile { get; set; }
        public Status Status { get; set; }
    }

    /// <summary>
    /// Post dto.
    /// </summary>
    public class PostDto
    {
        /// <summary>
        /// Gets or sets post.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets commentsCount.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets profile.
        /// </summary>
        public Profile Profile { get; set; }
    }
}