using Blog.Data.Models;

namespace Blog.Services.Dtos.Posts
{
    /// <summary>
    /// Comment dto.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// Gets or sets profile.
        /// </summary>
        public Profile Profile { get; set; }
    }
}
