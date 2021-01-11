using ClosedXML.Excel;

namespace Blog.Services.Core.Dtos
{
    /// <summary>
    /// WorkSheet dto.
    /// </summary>
    public class WorkSheetDto
    {
        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public IXLRows Rows { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkSheetDto"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        public string ExceptionMessage { get; set; }
    }
}