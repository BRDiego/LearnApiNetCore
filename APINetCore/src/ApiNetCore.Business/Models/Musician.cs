namespace ApiNetCore.Business.Models
{
    public class Musician : Entity
    {
        public string Name { get; set; } = "";
        public string Surnames { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string PictureFileName { get; set; } = "";
        public string Roles { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public List<Band> Bands { get; set; } = new List<Band>();

        public ushort Age {
            get 
            { 
                return (ushort)(DateTime.Now.Date.Year - DateOfBirth.Year);
            }
        }
    }
}