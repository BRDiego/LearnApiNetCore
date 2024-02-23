using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore.Business.Models
{
    public class Musician : Entity
    {
        [MinLength(1)]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; } = "";
        
        [MaxLength(50)]
        public string Surnames { get; set; } = "";
        
        [MaxLength(20)]
        public string Nickname { get; set; } = "";
        
        [MaxLength(100)]
        public string PictureFileName { get; set; } = "";

        [MaxLength(50)]
        public string Roles { get; set; } = "";

        [Required]
        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }
        public List<Band> Bands { get; set; } = new List<Band>();

        public ushort Age {
            get 
            { 
                return (ushort)(DateTime.Now.Date.Year - DateOfBirth.Year);
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Musician)
            {
                return false;
            }

            Musician other = (obj as Musician)!;

            return Name.Equals(other.Name)
                && DateOfBirth.Equals(other.DateOfBirth);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DateOfBirth);
        }

    }
}