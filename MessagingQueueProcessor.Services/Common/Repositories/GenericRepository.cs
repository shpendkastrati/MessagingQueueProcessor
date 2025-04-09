using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MessagingQueueProcessor.Data;
using Mapster;
using static MessagingQueueProcessor.Services.Common.Repositories.Interfaces.IGenericRepository;

namespace MessagingQueueProcessor.Services.Common.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
        where T : class
    {
        protected ApplicationDbContext Context { get; } = context;

        public async Task<ReadOnlyCollection<TResult>> GetListWithFilterAsync<TResult>(
            Expression<Func<T, bool>> criteria,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
            where TResult : class
        {
            var query = Context.Set<T>().Where(criteria);
            if (include != null)
            {
                query = include(query);
            }

            var result = await query.ProjectToType<TResult>().ToListAsync();
            return new ReadOnlyCollection<TResult>(result);
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = Context.Set<T>();
            return query.AnyAsync(criteria);
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = Context.Set<T>();
            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefaultAsync(predicate);
        }

        public Task<TResult?> GetAsync<TResult>(Expression<Func<T, bool>> predicate)
        {
            var query = Context.Set<T>().Where(predicate);
            return query.ProjectToType<TResult>().FirstOrDefaultAsync();
        }

        public Task<TResult?> GetAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var query = Context.Set<T>().Where(predicate);

            if (include != null)
            {
                query = include(query);
            }

            return query.ProjectToType<TResult>().FirstOrDefaultAsync();
        }

        public Task<TResult?> GetAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> projection)
        {
            var query = Context.Set<T>().Where(predicate);
            return query.Select(projection).FirstOrDefaultAsync();
        }

        public void Delete(T entity)
        {
            Context.Remove(entity);
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}

