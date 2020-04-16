using System.Collections.Generic;
using Blog.Core.HelperClasses;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    /// <summary>
    /// Posts view model.
    /// </summary>
    public class PostsViewModel
    {
        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<PostViewModel> Posts { get; set; }

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets onlyWithCommentsInfo.
        /// </summary>
        public bool OnlyWithCommentsInfo { get; set; }

        /// <summary>
        /// Gets or sets pageInfo.
        /// </summary>
        public PageInfo PageInfo { get; set; }
    }
}