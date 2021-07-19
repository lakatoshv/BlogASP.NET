using ClosedXML.Excel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Services.Core.Dtos.Users
{
    /// <summary>
    /// Role dto.
    /// </summary>
    /// <seealso cref="IdentityRole" />
    public class RoleDto : IdentityRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDto"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        public RoleDto(IXLRow row)
        {
            Name = row?.Cell(1).Value.ToString();
        }
    }
}