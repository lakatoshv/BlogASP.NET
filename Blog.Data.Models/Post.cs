using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Blog.Core.Enums;

namespace Blog.Data.Models
{
    /// <summary>
    /// Post model.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [AllowHtml]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets content.
        /// </summary>
        [AllowHtml]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets seen.
        /// </summary>
        public int Seen { get; set; }

        /// <summary>
        /// Gets or sets likes.
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Gets or sets dislikes.
        /// </summary>
        public int Dislikes { get; set; }

        /// <summary>
        /// Gets or sets createdAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets postTags.
        /// </summary>
        public ICollection<Tag> PostTags { get; set; }

        /// <summary>
        /// Gets or sets imageUrl.
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        public Status Status { get; set; } = Status.NotApproved;
    }
}