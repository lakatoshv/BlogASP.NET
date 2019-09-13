using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public Profile Profile { get; set; }
    }
}