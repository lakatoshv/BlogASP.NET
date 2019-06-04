using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Core.HelperClasses;

namespace Blog.ViewModels.Posts
{
    public class CommentsViewModel
    {
        public IList<CommentViewModel> Comments { get; set; }
        public string DisplayType { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}