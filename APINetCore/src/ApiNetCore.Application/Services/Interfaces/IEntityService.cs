using System.Linq.Expressions;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;
using ApiNetCore.Business.Models.Generic;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IEntityService<DtoType, EntityType> : IDisposable where DtoType : EntityDTO where EntityType : Entity
    {
        Task AddAsync(DtoType band);
        Task UpdateAsync(DtoType band);
        Task DeleteAsync(ushort id);
        Task<DtoType?> FindByIdAsync(ushort id);
        Task<IEnumerable<DtoType>> ListAsync();
        Task<IEnumerable<DtoType>> ListAsync(Expression<Func<EntityType, bool>> predicate);
    }
}