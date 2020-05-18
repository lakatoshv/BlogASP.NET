using Blog.Core.Enums;
using Blog.Data.Models;
using Blog.Services.Core.Dtos.Posts;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    /// <summary>
    /// Post show view model.
    /// </summary>
    public class PostViewModel
    {
        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>
        /// The post.
        /// </value>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public Comment Comment { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public CommentsViewModel Comments { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostViewModel"/> class.
        /// </summary>
        public PostViewModel()
        {
            Comments = new CommentsViewModel();
        }
    }
}