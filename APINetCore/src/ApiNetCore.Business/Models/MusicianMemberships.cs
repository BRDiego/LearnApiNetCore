namespace ApiNetCore.Business.Models
{
    public class MusicianMemberships
    {
        public Musician Musician = null!;
        public List<Band> Memberships { get; set; } = new List<Band>();
    }
}
