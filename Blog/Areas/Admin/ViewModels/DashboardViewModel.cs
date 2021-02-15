using System.Collections.Generic;
using Blog.Data.Models;

namespace Blog.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public IList<ApplicationUser> Users { get; set; }
        public IList<Post> Posts { get; set; }
        public IList<Tag> Tags { get; set; }

        public DashboardViewModel()
        {
            Users = new List<ApplicationUser>();
            Posts = new List<Post>();
            Tags = new List<Tag>();
        }
    }
}