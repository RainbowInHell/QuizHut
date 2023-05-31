using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace QuizHut.DLL.Repositories.Contracts
{
    public interface IRepository<TEntity> : IDisposable 
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();

        Task<TEntity> GetByIdAsync(string id);

        Task<int> CountAsync();

        Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector);

        Task<int> SumAsync(Expression<Func<TEntity, int>> selector);

        Task<int> MinAsync(Expression<Func<TEntity, int>> selector);

        Task<int> MaxAsync(Expression<Func<TEntity, int>> selector);

        Task<long> LongCountAsync();
    }
}