using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    /// <summary>
    /// Comment with posts view model.
    /// </summary>
    public class CommentWithPostsViewModel
    {
        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public List<Post> Posts { get; internal set; }

        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        public Comment Comment { get; internal set; }
    }

    /// <summary>
    /// Comment with post view model.
    /// </summary>
    public class CommentWithPostViewModel
    {
        /// <summary>
        /// Gets or sets post.
        /// </summary>
        public PostViewModel Post { get; internal set; }

        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        public CommentViewModel Comment { get; internal set; }
    }
}