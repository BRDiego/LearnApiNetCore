namespace ApiNetCore.Business.Models
{
    public class Band : Entity
    {
        public string Name { get; set; } = "";
        public string MusicalStyles { get; set; } = "";
        public string ImageFileName { get; set; } = "";
        public List<Musician> Musicians { get; set; } = new List<Musician>();
    }
}