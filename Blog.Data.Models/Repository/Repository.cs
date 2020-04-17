using Blog.Data.Models.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.Data.Models.Repository
{
    /// <summary>
    /// Repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity}" />
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly BlogContext _dbContext;

        /// <summary>
        /// The entities
        /// </summary>
        private DbSet<TEntity> _entities;

        /// <inheritdoc/>
        public IQueryable<TEntity> Table => _entities;

        /// <inheritdoc/>
        public IQueryable<TEntity> TableNoTracking => _entities.AsNoTracking();

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        protected virtual DbSet<TEntity> Entities => _entities ?? (_entities = _dbContext.Set<TEntity>());

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public Repository(BlogContext db)
        {
            _dbContext = db;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAll()
        {
            return Entities.AsQueryable();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.Where(expression);
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await Entities.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Entities.Where(expression).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                Entities.AddRange(entities);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Add(entity);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                Entities.AddRange(entities);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                _dbContext.Entry(entities).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                _dbContext.Entry(entities).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                Entities.RemoveRange(entities);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Remove(entity);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                Entities.RemoveRange(entities);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc/>
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return Entities.Any(expression);
        }

        /// <inheritdoc/>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return Entities.FirstOrDefault(expression);
        }

        /// <inheritdoc/>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return await Entities.FirstOrDefaultAsync(expression);
        }

        /// <inheritdoc/>
        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return Entities.LastOrDefault(expression);
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return Entities.Where(expression);
        }

        /// <inheritdoc/>
        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw  new ArgumentNullException(nameof(expression));
            }

            return Entities.Count(expression);
        }

        /// <inheritdoc/>
        public SelectList GetTableSelectList(string valueField, string fieldToDisplay, object selectedValue)
        {
            if (selectedValue != null)
            {
                return new SelectList(Entities, valueField, fieldToDisplay, selectedValue);
            }

            return new SelectList(Entities, valueField, fieldToDisplay);
        }

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
                _dbContext.Dispose();
            }
        }

        /// <summary>
        /// Gets the full error text and rollback entity changes.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            if (_dbContext != null)
            {
                var entries = _dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                    .ToList();

                entries.ForEach(entry => entry.State = EntityState.Unchanged);
            }

            _dbContext?.SaveChanges();
            return exception?.ToString();
        }
    }
}