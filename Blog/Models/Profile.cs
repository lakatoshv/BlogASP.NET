﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string ApplicationUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImg { get; set; }
    }
}