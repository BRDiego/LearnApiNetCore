using FluentValidation;

namespace ApiNetCore.Application.DTOs.Models
{
    public abstract class EntityDTO
    {
        public ushort Id { get; set; }
    }
}