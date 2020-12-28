using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Popular items dto.
    /// </summary>
    public class PopularItemsDto
    {
        /// <summary>
        /// Gets or sets the popular posts.
        /// </summary>
        /// <value>
        /// The popular posts.
        /// </value>
        public List<Post> PopularPosts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopularItemsDto"/> class.
        /// </summary>
        public PopularItemsDto()
        {
            PopularPosts = new List<Post>();
        }
    }
}