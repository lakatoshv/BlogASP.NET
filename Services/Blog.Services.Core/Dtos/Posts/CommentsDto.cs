using Blog.Core.HelperClasses;
using System.Collections.Generic;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Comments dto.
    /// </summary>
    public class CommentsDto
    {
        /// <summary>
        /// Gets or sets comments.
        /// </summary>
        public IList<CommentDto> Comments { get; set; }

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