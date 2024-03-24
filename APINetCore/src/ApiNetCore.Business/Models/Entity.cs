using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Business.Models
{
    public abstract class Entity
    {
        [Key]
        public ushort Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime LastChangeAt { get; set; } = DateTime.Now;
        [Required]
        public bool Revoked { get; set; } = false;
    }
}