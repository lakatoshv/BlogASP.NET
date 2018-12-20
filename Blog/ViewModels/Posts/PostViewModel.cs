using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels.Posts
{
    public class PostViewModel
    {
        public Post post { get; set; }
        public IList<Comment> comments { get; set; }
    }
}