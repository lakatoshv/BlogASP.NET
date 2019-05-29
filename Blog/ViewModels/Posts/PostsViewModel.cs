using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Core.HelperClasses;
using PagedList;

namespace Blog.ViewModels.Posts
{
    public class PostsViewModel
    {
        public IList<PostViewModel> Posts { get; set; }
        public string DisplayType { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}