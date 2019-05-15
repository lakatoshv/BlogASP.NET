using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Core.Dtos
{
    public class SortParametersDto
    {
        public string OrderBy { get; set; }
        public  string SortBy { get; set; }
    }
}