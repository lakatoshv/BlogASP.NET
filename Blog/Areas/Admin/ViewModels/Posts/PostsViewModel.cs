using System.Collections.Generic;
using Blog.Core.HelperClasses;
using Blog.Data.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
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
        public IList<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the display type.
        /// </summary>
        /// <value>
        /// The display type.
        /// </value>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [only with comments information].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [only with comments information]; otherwise, <c>false</c>.
        /// </value>
        public bool OnlyWithCommentsInfo { get; set; }

        /// <summary>
        /// Gets or sets the page information.
        /// </summary>
        /// <value>
        /// The page information.
        /// </value>
        public PageInfo PageInfo { get; set; }
    }
}