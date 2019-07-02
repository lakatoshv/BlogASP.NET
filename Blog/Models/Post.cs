using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Blog.Models
{
    public class Post
    {
        /*
        id -> int, increment
        title -> string
        description -> string/text
        content -> string/text
        author  -> string/text
        seen -> int
        likes -> int
        dislikes -> int
        tags -> string
        imgurl -> string
        //access -> string
        */
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Author { get; set; }
        public int Seen { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Tags { get; set; }
        public ICollection<Tag> PostTags { get; set; }
        public string Imgurl { get; set; }
    }
}