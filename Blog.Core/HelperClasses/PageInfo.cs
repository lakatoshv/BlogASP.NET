using System;

namespace Blog.Core.HelperClasses
{
    /// <summary>
    /// Page Info.
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Gets or sets current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets total items count.
        /// </summary>
        public int TotalItems { get; set; }

        // get { return (int)Math.Ceiling((decimal)this.TotalItems / this.PageSize); }
        /// <summary>
        /// Gets total pages count.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}