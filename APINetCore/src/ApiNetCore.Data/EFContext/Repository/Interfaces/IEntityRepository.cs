using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IEntityRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetAll();
        Task Update(TEntity entity);
        Task Delete(int id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
    }
}