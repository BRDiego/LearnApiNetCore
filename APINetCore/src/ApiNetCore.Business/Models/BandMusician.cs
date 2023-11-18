namespace ApiNetCore.Business.Models
{
    public class BandMusician
    {
        public List<Band> Bands { get; set; } = new List<Band>();
        public List<Musician> Musicians { get; set; } = new List<Musician>();
    }
}