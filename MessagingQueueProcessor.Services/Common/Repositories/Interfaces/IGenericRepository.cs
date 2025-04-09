using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MessagingQueueProcessor.Services.Common.Repositories.Interfaces
{
    public interface IGenericRepository
    {
        public interface IGenericRepository<T>
        where T : class
        {
            Task<ReadOnlyCollection<TResult>> GetListWithFilterAsync<TResult>(
                Expression<Func<T, bool>> criteria,
                Func<IQueryable<T>, IQueryable<T>>? include = null)
                where TResult : class;

            void Add(T entity);

            Task<bool> ExistsAsync(Expression<Func<T, bool>> criteria);

            void Update(T entity);

            Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null);

            Task<TResult?> GetAsync<TResult>(Expression<Func<T, bool>> predicate);

            Task<TResult?> GetAsync<TResult>(
                Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IQueryable<T>>? include = null);

            Task<TResult?> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> projection);

            Task SaveChangesAsync();

            void Delete(T entity);
        }
    }
}
