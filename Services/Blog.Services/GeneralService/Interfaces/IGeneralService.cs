﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Services.GeneralService.Interfaces
{
    /// <summary>
    /// General service interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IDisposable" />
    public interface IGeneralService<T> : IDisposable
    {
        /// <summary>
        /// Finds the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        T Find(object id);

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task<T> FindAsync(object id);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(T entity);

        /// <summary>
        /// Inserts the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task InsertAsync(T entity);

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Task.</returns>
        Task InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(IEnumerable<T> entity);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IQueryable.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>IQueryable.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task<IList<T>> GetAllAsync();

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Task.</returns>
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Any the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>bool.</returns>
        bool Any(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>T.</returns>
        T FirstOrDefault(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Firsts the or default asynchronous.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Lasts the or default.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>T.</returns>
        T LastOrDefault(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Wheres the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Counts the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>int.</returns>
        int Count(Expression<Func<T, bool>> expression);
    }
}