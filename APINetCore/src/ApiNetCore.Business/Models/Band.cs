using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Business.Models
{
    public class Band : Entity
    {
        [MinLength(1)]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; } = "";

        [MinLength(3)]
        [MaxLength(50)]
        [Required]
        public string MusicalStyles { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
        public List<Musician> Musicians { get; set; } = new List<Musician>();

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Band)
            {
                return false;
            }

            Band other = (obj as Band)!;

            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }
}