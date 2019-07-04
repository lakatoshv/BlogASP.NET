using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    public class PostShowViewModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
        public CommentsViewModel Comments { get; set; }
        public int CommentsCount { get; set; }
        public Profile Profile { get; set; }
    }

    public class PostViewModel
    {
        public Post Post { get; set; }
        public int CommentsCount { get; set; }
        public Profile Profile { get; set; }
    }
}