using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels.Posts
{
    public class PostShowViewModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
        public IList<CommentViewModel> Comments { get; set; }
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