﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Core.Attributes
{
    public class StringValueAttribute : System.Attribute
    {

        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
}