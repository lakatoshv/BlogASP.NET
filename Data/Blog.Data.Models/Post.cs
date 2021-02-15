using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Blog.Core.Enums;
using Blog.Data.Core.Models;

namespace Blog.Data.Models
{
    /// <summary>
    /// Post entity.
    /// </summary>
    public class Post : BaseDeletableModel<int>
    {
        //access -> string
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [AllowHtml]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [AllowHtml]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [ForeignKey("Author")]
        [Required]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public virtual ApplicationUser Author { get; set; }

        /// <summary>
        /// Gets or sets the seen.
        /// </summary>
        /// <value>
        /// The seen.
        /// </value>
        public int Seen { get; set; } = 0;

        /// <summary>
        /// Gets or sets the likes.
        /// </summary>
        /// <value>
        /// The likes.
        /// </value>
        public int Likes { get; set; } = 0;

        /// <summary>
        /// Gets or sets the dislikes.
        /// </summary>
        /// <value>
        /// The dislikes.
        /// </value>
        public int Dislikes { get; set; } = 0;

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; } = Status.NotApproved;

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the post tags.
        /// </summary>
        /// <value>
        /// The post tags.
        /// </value>
        public virtual ICollection<Tag> PostTags { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public virtual ICollection<Comment> Comments { get; set; }
    }
}