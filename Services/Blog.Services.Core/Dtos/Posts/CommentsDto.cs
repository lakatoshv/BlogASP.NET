using Blog.Core.HelperClasses;
using System.Collections.Generic;
using Blog.Data.Models;

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
        public IList<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets pageInfo.
        /// </summary>
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsDto"/> class.
        /// </summary>
        public CommentsDto()
        {
            Comments = new List<Comment>();
        }
    }
}