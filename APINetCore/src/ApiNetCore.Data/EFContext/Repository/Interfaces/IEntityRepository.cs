using System.Linq.Expressions;
using ApiNetCore.Business.Models.Generic;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IEntityRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<int> SaveChangesAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(ushort id);
        Task<TEntity?> FindByIdAsync(ushort id);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> ListAsync();
        Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);
    }
}