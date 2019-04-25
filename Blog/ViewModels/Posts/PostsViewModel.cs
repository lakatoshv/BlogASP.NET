using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels.Posts
{
    public class MyPostsViewModel
    {
        public IList<PostViewModel> Posts { get; set; }
        public string DisplayType { get; set; }
    }
}