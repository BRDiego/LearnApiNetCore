namespace ApiNetCore.Application.DTOs.Models
{
    public class MusicianMembershipsDTO
    {
        public MusicianDTO Musician { get; set; } = null!;
        public IEnumerable<BandDTO> Bands { get; set; } = new List<BandDTO>();
    }
}
