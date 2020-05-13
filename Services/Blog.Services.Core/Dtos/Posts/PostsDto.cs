using System.Collections.Generic;
using Blog.Core.HelperClasses;
using Blog.Data.Models;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Posts dto.
    /// </summary>
    public class PostsDto
    {
        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets pageInfo.
        /// </summary>
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Gets or sets onlyWithCommentsInfo.
        /// </summary>
        public bool OnlyWithCommentsInfo { get; set; }
    }
}