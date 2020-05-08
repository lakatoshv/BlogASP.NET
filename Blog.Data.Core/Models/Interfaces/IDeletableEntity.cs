using System;

namespace Blog.Data.Core.Models.Interfaces
{
    /// <summary>
    /// Deletable entity interface.
    /// </summary>
    public interface IDeletableEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether is deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets deleted on.
        /// </summary>
        DateTime? DeletedOn { get; set; }
    }
}