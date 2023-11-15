namespace ApiNetCore.Business.Models
{
    public class Musician : Entity
    {
        public string Name { get; set; } = "";
        public string Surnames { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Roles { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public List<Band> Bands { get; set; } = new List<Band>();
    }
}