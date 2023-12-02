namespace ApiNetCore.Application.DTOs
{
    public class BandDTO : EntityDTO
    {
        public string Name { get; set; } = "";
        public string MusicalStyles { get; set; } = "";
        public string ImageFileName { get; set; } = "";
    
        public List<MusicianDTO> Musicians { get; set; } = new List<MusicianDTO>();
    }
}