using System.Collections.Generic;
using Blog.Core.HelperClasses;
using Blog.Data.Models;

namespace Blog.ViewModels.Posts
{
    /// <summary>
    /// Comments view model.
    /// </summary>
    public class CommentsViewModel
    {
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public IList<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the display type.
        /// </summary>
        /// <value>
        /// The display type.
        /// </value>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets the page information.
        /// </summary>
        /// <value>
        /// The page information.
        /// </value>
        public PageInfo PageInfo { get; set; }
    }
}