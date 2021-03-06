﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Data.Core.Models;

namespace Blog.Data.Models
{
    /// <summary>
    /// Message entity.
    /// </summary>
    public class Message : BaseDeletableModel<int>
    {
        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>
        /// The sender identifier.
        /// </value>
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public virtual ApplicationUser Sender { get; set; }

        /// <summary>
        /// Gets or sets the recipient identifier.
        /// </summary>
        /// <value>
        /// The recipient identifier.
        /// </value>
        public string RecipientId { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>
        /// The recipient.
        /// </value>
        public virtual ApplicationUser Recipient { get; set; }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        /// <value>
        /// The sender email.
        /// </value>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender.
        /// </summary>
        /// <value>
        /// The name of the sender.
        /// </value>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }
    }
}