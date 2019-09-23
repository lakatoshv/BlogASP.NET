using System.Collections.Generic;
using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    public class CommentWithPostsViewModel
    {
        public List<Post> Posts { get; internal set; }
        public Comment Comment { get; internal set; }
    }
    public class CommentWithPostViewModel
    {
        public PostViewModel Post { get; internal set; }
        public CommentViewModel Comment { get; internal set; }
    }
}