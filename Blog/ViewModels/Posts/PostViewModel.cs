﻿using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.ViewModels.Posts
{
    public class PostViewModel
    {
        public Post post { get; set; }
        public Comment comment { get; set; }
        public IList<CommentViewModel> comments { get; set; }
        public int CommentsCount { get; set; }
        public Profile Profile { get; set; }
    }
}