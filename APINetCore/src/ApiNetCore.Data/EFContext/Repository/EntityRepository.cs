using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;
        
        public EntityRepository(ApplicationDbContext context)
        {
            dbContext = context;
            dbSet = dbContext.Set<TEntity>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        public async Task AddAsync(TEntity entity)
        {
            dbSet.Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            await SaveChangesAsync();
        }
        
        public async Task DeleteAsync(ushort id)
        {
            dbSet.Remove(new TEntity { Id = id });
            await SaveChangesAsync();
        }

        public async Task<TEntity> FindAsync(ushort id)
        {
            return await dbSet.FirstAsync(x => x.Id == id);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().FirstAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
    }
}