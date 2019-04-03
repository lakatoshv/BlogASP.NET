using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Models;

namespace Blog.ViewModels.Users
{
    public class ProfileViewModel
    {
        public ApplicationUser UserData { get; set; }
        public Profile ProfileData { get; set; }
        public IList<Post> Posts { get; set; }
    }
}