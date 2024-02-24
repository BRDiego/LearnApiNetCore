using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Business.Models
{
    public abstract class Entity
    {
        [Key]
        public ushort Id { get; set; }
    }
}