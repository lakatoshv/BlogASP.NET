using Blog.Data.Models;
using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    /// <summary>
    /// Comment view model.
    /// </summary>
    public class CommentViewModel
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