using Blog.Core.Enums;
using Blog.Data.Models;
using Blog.Models;
using Blog.Services.Core.Dtos.Posts;

namespace Blog.ViewModels.Posts
{
    /// <summary>
    /// 
    /// </summary>
    public class PostViewModel
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
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostShowDto"/> class.
        /// </summary>
        public PostViewModel()
        {
            Comments = new CommentsViewModel();
        }
    }
}