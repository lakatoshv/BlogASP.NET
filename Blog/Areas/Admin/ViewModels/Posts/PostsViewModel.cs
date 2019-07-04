using System.Collections.Generic;
using Blog.Core.HelperClasses;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    public class PostsViewModel
    {
        public IList<PostViewModel> Posts { get; set; }
        public string DisplayType { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}