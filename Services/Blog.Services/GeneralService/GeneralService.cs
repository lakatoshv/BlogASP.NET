using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Services.GeneralService.Interfaces;
using BLog.Data.Repository.Interfaces;

namespace Blog.Services.GeneralService
{
    /// <summary>
    /// General service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IGeneralService{T}" />
    public class GeneralService<T> : IGeneralService<T>
        where T : class
    {
        /// <summary>
        /// The repository
        /// </summary>
        protected IRepository<T> Repository;
        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        protected IQueryable<T> Table => Repository.Table;

        /// <summary>
        /// Gets the table no tracking.
        /// </summary>
        /// <value>
        /// The table no tracking.
        /// </value>
        protected IQueryable<T> TableNoTracking => Repository.TableNoTracking;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralService{T}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public GeneralService(IRepository<T> repository)
        {
            Repository = repository;
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public T Find(object id) => Repository.GetById(id);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<T> FindAsync(object id) => await Repository.GetByIdAsync(id).ConfigureAwait(false);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Insert(T entity) => Repository.Insert(entity);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Insert(IEnumerable<T> entities) => Repository.Insert(entities);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task InsertAsync(T entity) => await Repository.InsertAsync(entity).ConfigureAwait(false);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task InsertAsync(IEnumerable<T> entities) =>
            await Repository.InsertAsync(entities).ConfigureAwait(false);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Update(T entity) => Repository.Update(entity);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Update(IEnumerable<T> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Repository.Update(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task UpdateAsync(T entity) => await Repository.UpdateAsync(entity).ConfigureAwait(false);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await Repository.UpdateAsync(entities).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Delete(int id)
        {
            var entity = Find(id);
            Delete(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Delete(T entity) => Repository.Delete(entity);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Delete(IEnumerable<T> entities) => Repository.Delete(entities);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task DeleteAsync(int id)
        {
            var entity = await FindAsync(id).ConfigureAwait(false);
            await DeleteAsync(entity).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task DeleteAsync(T entity) => await Repository.DeleteAsync(entity);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task DeleteAsync(IEnumerable<T> entities) => await Repository.DeleteAsync(entities);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public IQueryable<T> GetAll() => Repository.GetAll();

        /// <inheritdoc cref="IGeneralService{T}"/>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression) => Repository.GetAll(expression);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<IList<T>> GetAllAsync() => await Repository.GetAllAsync().ConfigureAwait(false);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression) =>
            await Repository.GetAllAsync(expression).ConfigureAwait(false);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public bool Any(Expression<Func<T, bool>> expression) => Repository.Any(expression);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public T FirstOrDefault(Expression<Func<T, bool>> expression) => Repository.FirstOrDefault(expression);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression) =>
            await Repository.FirstOrDefaultAsync(expression);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public T LastOrDefault(Expression<Func<T, bool>> expression) => Repository.LastOrDefault(expression);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public IQueryable<T> Where(Expression<Func<T, bool>> expression) => Repository.Where(expression);

        /// <inheritdoc cref="IGeneralService{T}"/>
        public int Count(Expression<Func<T, bool>> expression) => Repository.Count(expression);

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Repository.Dispose();
            }
        }
    }
}