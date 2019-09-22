using System.Collections.Generic;
using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    public class CommentWithPostViewModel
    {
        public List<Post> Posts { get; internal set; }
        public Comment Comment { get; internal set; }
    }
}