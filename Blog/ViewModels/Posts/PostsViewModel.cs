using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels.Posts
{
    public class PostsViewModel
    {
        public Post Post { get; set; }
        public int CommentsCount { get; set; }
        public Profile Profile { get; set; }
    }
}