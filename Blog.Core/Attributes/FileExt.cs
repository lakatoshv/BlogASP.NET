using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Blog.Core.Attributes
{
    /// <summary>
    /// FileExt attribute.
    /// </summary>
    public class FileExt : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the allow.
        /// </summary>
        /// <value>
        /// The allow.
        /// </value>
        public string Allow { get; set; }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="validationContext">validation context.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var extension = ((HttpPostedFileBase) value).FileName.Split('.')[1];

            return Allow.Contains(extension)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}