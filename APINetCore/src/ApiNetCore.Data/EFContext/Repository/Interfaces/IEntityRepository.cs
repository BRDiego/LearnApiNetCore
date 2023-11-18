using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IEntityRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(ushort id);
        Task<List<TEntity>> ListAllAsync();
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(ushort id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}