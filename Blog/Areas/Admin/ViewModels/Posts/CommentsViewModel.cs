using System.Collections.Generic;
using Blog.Core.HelperClasses;

namespace Blog.Areas.Admin.ViewModels.Posts
{
    public class CommentsViewModel
    {
        public IList<CommentViewModel> Comments { get; set; }
        public string DisplayType { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}