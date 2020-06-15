using Blog.Core.Enums;
using Blog.Data.Models;

namespace Blog.Services.Core.Dtos.Posts
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
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostShowDto"/> class.
        /// </summary>
        public PostShowDto()
        {
            Comments = new CommentsDto();
        }
    }
}