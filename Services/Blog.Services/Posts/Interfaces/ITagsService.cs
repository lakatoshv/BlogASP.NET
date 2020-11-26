﻿using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.GeneralService.Interfaces;

namespace Blog.Services.Posts.Interfaces
{
    /// <summary>
    /// Tags service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{T}" />
    public interface ITagsService : IGeneralService<Tag>
    {
        /// <summary>
        /// Deletes the post tags.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task DeletePostTags(int id);
    }
}