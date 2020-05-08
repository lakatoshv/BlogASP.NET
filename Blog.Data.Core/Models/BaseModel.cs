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
        protected BaseModel(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Key]
        public TKey Id { get; }

        /// <summary>
        /// Gets or sets created on.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets modified on.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}