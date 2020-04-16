using Blog.Data.Models;

namespace Blog.ViewModels.Posts
{
    /// <summary>
    /// Post show view model.
    /// </summary>
    public class PostShowViewModel
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
        public CommentsViewModel Comments { get; set; }

        /// <summary>
        /// Gets or sets commentsCount.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets profile.
        /// </summary>
        public Profile Profile { get; set; }
    }

    /// <summary>
    /// Post view model.
    /// </summary>
    public class PostViewModel
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