using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels.Posts
{
    public class CommentViewModel
    {
        public Comment comment { get; set; }
        public Profile Profile { get; set; }
    }
}