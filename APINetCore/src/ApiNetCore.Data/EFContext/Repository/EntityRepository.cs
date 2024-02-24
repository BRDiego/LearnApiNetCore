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

        public async Task<TEntity?> FindByIdAsync(ushort id)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}