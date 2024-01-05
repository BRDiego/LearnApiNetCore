using FluentValidation;

namespace ApiNetCore.Application.DTOs
{
    public abstract class EntityDTO
    {
        public ushort Id { get; set; }
    }
}