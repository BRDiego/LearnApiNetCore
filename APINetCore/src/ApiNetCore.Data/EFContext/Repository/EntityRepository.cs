using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;
        
        public EntityRepository(ApplicationDbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChangesAsync();
        }
        
        public async Task DeleteAsync(ushort id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(ushort id)
        {
            return await DbSet.FirstAsync(x => x.Id == id);
        }

        public async Task<List<TEntity>> ListAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}