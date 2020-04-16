using System.Collections.Generic;
using Blog.Core.HelperClasses;

namespace Blog.ViewModels.Posts
{
    /// <summary>
    /// Comments view model.
    /// </summary>
    public class CommentsViewModel
    {
        /// <summary>
        /// Gets or sets comments.
        /// </summary>
        public IList<CommentViewModel> Comments { get; set; }

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets pageInfo.
        /// </summary>
        public PageInfo PageInfo { get; set; }
    }
}