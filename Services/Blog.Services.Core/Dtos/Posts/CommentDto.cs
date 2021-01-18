using Blog.Data.Models;
using ClosedXML.Excel;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Comment dto.
    /// </summary>
    /// <seealso cref="Comment" />
    public class CommentDto : Comment
    {
        /// <summary>
        /// Gets or sets the post title.
        /// </summary>
        /// <value>
        /// The post title.
        /// </value>
        public string PostTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDto"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        public CommentDto(IXLRow row)
        {
            PostTitle = row?.Cell(1).Value.ToString();
            CommentBody = row?.Cell(2).Value.ToString();
        }
    }
}