using Blog.Data.Models.Repository.Interfaces;
using Blog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Services
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

        /// <inheritdoc/>
        public T Find(object id) => Repository.GetById(id);

        /// <inheritdoc/>
        public async Task<T> FindAsync(object id) => await Repository.GetByIdAsync(id).ConfigureAwait(false);

        /// <inheritdoc/>
        public void Insert(T entity) => Repository.Insert(entity);

        /// <inheritdoc/>
        public void Insert(IEnumerable<T> entities) => Repository.Insert(entities);

        /// <inheritdoc/>
        public async Task InsertAsync(T entity) => await Repository.InsertAsync(entity).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task InsertAsync(IEnumerable<T> entities) =>
            await Repository.InsertAsync(entities).ConfigureAwait(false);

        /// <inheritdoc/>
        public void Update(T entity) => Repository.Update(entity);

        /// <inheritdoc/>
        public void Update(IEnumerable<T> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Repository.Update(entities);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(T entity) => await Repository.UpdateAsync(entity).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await Repository.UpdateAsync(entities).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            var entity = Find(id);
            Delete(entity);
        }

        /// <inheritdoc/>
        public void Delete(T entity) => Repository.Delete(entity);

        /// <inheritdoc/>
        public void Delete(IEnumerable<T> entities) => Repository.Delete(entities);

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var entity = await FindAsync(id).ConfigureAwait(false);
            await DeleteAsync(entity).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(T entity) => await Repository.DeleteAsync(entity);

        /// <inheritdoc/>
        public async Task DeleteAsync(IEnumerable<T> entities) => await Repository.DeleteAsync(entities);

        /// <inheritdoc/>
        public IQueryable<T> GetAll() => Repository.GetAll();

        /// <inheritdoc/>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression) => Repository.GetAll(expression);

        /// <inheritdoc/>
        public async Task<IList<T>> GetAllAsync() => await Repository.GetAllAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression) =>
            await Repository.GetAllAsync(expression).ConfigureAwait(false);

        /// <inheritdoc/>
        public bool Any(Expression<Func<T, bool>> expression) => Repository.Any(expression);

        /// <inheritdoc/>
        public T FirstOrDefault(Expression<Func<T, bool>> expression) => Repository.FirstOrDefault(expression);

        /// <inheritdoc/>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression) => 
            await Repository.FirstOrDefaultAsync(expression);

        /// <inheritdoc/>
        public T LastOrDefault(Expression<Func<T, bool>> expression) => Repository.LastOrDefault(expression);

        /// <inheritdoc/>
        public IQueryable<T> Where(Expression<Func<T, bool>> expression) => Repository.Where(expression);

        /// <inheritdoc/>
        public int Count(Expression<Func<T, bool>> expression) => Repository.Count(expression);

        /// <inheritdoc/>
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
