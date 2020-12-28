using System.Collections.Generic;
using Blog.Core.HelperClasses;

namespace Blog.ViewModels.Posts
{
    /// <summary>
    /// Posts view model.
    /// </summary>
    public class PostsViewModel
    {
        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public IList<PostViewModel> Posts { get; set; }

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