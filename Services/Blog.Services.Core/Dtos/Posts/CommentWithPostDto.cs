using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Comment with posts dto.
    /// </summary>
    public class CommentWithPostsDto
    {
        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public List<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public Comment Comment { get; set; }
    }

    /// <summary>
    /// Comment with post dto.
    /// </summary>
    public class CommentWithPostDto
    {
        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>
        /// The post.
        /// </value>
        public PostDto Post { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public CommentDto Comment { get; set; }
    }
}