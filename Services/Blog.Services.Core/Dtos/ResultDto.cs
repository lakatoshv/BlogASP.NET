namespace Blog.Services.Core.Dtos
{
    /// <summary>
    /// Result Dto.
    /// </summary>
    public class ResultDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ResultDto"/> is success.
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