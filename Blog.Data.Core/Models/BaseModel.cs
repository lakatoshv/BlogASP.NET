using System;
using System.ComponentModel.DataAnnotations;
using Blog.Data.Core.Models.Interfaces;

namespace Blog.Data.Core.Models
{
    /// <summary>
    /// Base model.
    /// </summary>
    /// <typeparam name="TKey">TKey.</typeparam>
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        /// <inheritdoc cref="IAuditInfo"/>
        [Key]
        public TKey Id { get; set; }

        /// <inheritdoc cref="IAuditInfo"/>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <inheritdoc cref="IAuditInfo"/>
        public DateTime? ModifiedOn { get; set; }
    }
}