using System.ComponentModel.DataAnnotations;
using System.Web;
using Blog.Core.Attributes;

namespace Blog.Areas.Admin.ViewModels
{
    /// <summary>
    /// Upload file view model.
    /// </summary>
    public class UploadFileViewModel
    {
        /// <summary>
        /// Gets or sets the excel file.
        /// </summary>
        /// <value>
        /// The excel file.
        /// </value>
        [Required(ErrorMessage = "Please select file.")]
        [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file.")]
        public HttpPostedFileBase ExcelFile { get; set; }
    }
}