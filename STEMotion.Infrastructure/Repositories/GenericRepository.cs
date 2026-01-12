using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StemotionContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(StemotionContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Creates a new entity in the database asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity) => _dbSet.Remove(entity);

        /// <summary>
        /// Checks if any entity exists in the database that matches the given predicate asynchronously. Predicate is a condition to filter entities.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

        /// <summary>
        /// Collects all entities that match the given predicate asynchronously. Predicate is a condition to filter entities.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();


        /// <summary>
        /// Collects the first entity that matches the given predicate asynchronously. Predicate is a condition to filter entities. Predicate is a condition to filter entities.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>

        //public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(predicate);
        }


        /// <summary>
        /// Gets all entities from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Kiểm tra xem includes có giá trị và có phần tử nào không
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets an entity by its unique identifier asynchronously. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity) =>  _dbSet.Update(entity);
    }
}
