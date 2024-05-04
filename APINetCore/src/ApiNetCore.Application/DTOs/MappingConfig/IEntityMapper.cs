using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models.Generic;
namespace ApiNetCore.Application.DTOs.MappingConfig
{
    public interface IEntityMapper<TDto, TEntity> where TDto : EntityDTO where TEntity : Entity
    {
        TEntity ToEntity(TDto entityDTO);
        TDto ToDto(TEntity? entity);
        IEnumerable<TDto> ToDto(IEnumerable<TEntity> entity);
    }
}
