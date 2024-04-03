using ApiNetCore.Application.DTOs.Models;
using FluentValidation;

namespace ApiNetCore.Application.DTOs.Interfaces
{
    public interface IValidDtoEntity<DtoType> where DtoType : EntityDTO
    {
        public AbstractValidator<DtoType> GetFluentValidator();
    }
}