using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Services.Dtos.Posts
{
    public class CommentWithPostsDto
    {
        public List<Post> Posts { get; set; }
        public Comment Comment { get; set; }
    }
    public class CommentWithPostDto
    {
        public PostDto Post { get; set; }
        public CommentDto Comment { get; set; }
    }
}