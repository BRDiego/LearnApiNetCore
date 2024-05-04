namespace ApiNetCore.Application.DTOs.Models
{
    public class MusicianMembershipsDTO : MusicianDTO
    {
        public IEnumerable<BandDTO> Bands { get; set; } = new List<BandDTO>();
    }
}
