using System;
using System.Collections.Generic;
using Blog.Core.Enums;
using Blog.Data.Models;
using ClosedXML.Excel;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Post show dto.
    /// </summary>
    public class PostShowDto
    {
        /// <summary>
        /// Gets or sets post.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// Gets or sets comments.
        /// </summary>
        public CommentsDto Comments { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostShowDto"/> class.
        /// </summary>
        public PostShowDto()
        {
            Comments = new CommentsDto();
        }
    }

    /// <summary>
    /// Post dto
    /// </summary>
    /// <seealso cref="Post" />
    public sealed class PostDto : Post
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostDto"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        public PostDto(IXLRow row)
        {
            Title = row?.Cell(1).Value.ToString();
            Description = row?.Cell(2).Value.ToString();
            Content = row?.Cell(3).Value.ToString();
            ImageUrl = row?.Cell(4).Value.ToString();
            Status = (Status) Enum.Parse(typeof(Status), row?.Cell(5).Value.ToString() ?? Status.NotApproved.ToString());

            IList<Tag> tags = new List<Tag>();
            for (var index = 0;
                index < row?.Cell(6).Value.ToString().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                index++)
            {
                var tag = row.Cell(6).Value.ToString().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[index];
                var tagForAdd = new Tag()
                {
                    Title = tag,
                };
                tags.Add(tagForAdd);
            }

            PostTags = tags;
        }
    }
}