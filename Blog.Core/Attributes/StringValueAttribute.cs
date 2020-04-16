namespace Blog.Core.Attributes
{
    /// <summary>
    /// String value attribute.
    /// </summary>
    public class StringValueAttribute : System.Attribute
    {
        /// <summary>
        /// Gets or sets value;
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Initializes static members of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

    }
}