namespace Blog.Services.Core.Dtos
{
    /// <summary>
    /// Sort parameters dto.
    /// </summary>
    public class SortParametersDto
    {
        /// <summary>
        /// Gets or sets orderBy.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets sortBy.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }
    }
}