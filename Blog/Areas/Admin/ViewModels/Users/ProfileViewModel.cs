using System.Collections.Generic;
using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Users
{
    public class ProfileViewModel
    {
        public ApplicationUser UserData { get; set; }
        public Profile ProfileData { get; set; }
        public IList<Post> Posts { get; set; }
    }
}