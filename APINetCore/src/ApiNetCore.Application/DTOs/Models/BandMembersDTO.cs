namespace ApiNetCore.Application.DTOs.Models
{
    public class BandMembersDTO
    {
        public BandDTO Band { get; set; } = null!;
        public IEnumerable<MusicianDTO> Members { get; set; } = new List<MusicianDTO>();
    }
}
