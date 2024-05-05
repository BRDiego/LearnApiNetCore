namespace ApiNetCore.Business.Models
{
    public class BandMembers
    {
        public Band Band = null!;
        public List<Musician> Members { get; set; } = new List<Musician>();
    }
}
