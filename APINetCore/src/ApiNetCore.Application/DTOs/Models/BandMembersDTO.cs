namespace ApiNetCore.Application.DTOs.Models
{
    public class BandMembersDTO : BandDTO
    {
        public IEnumerable<MusicianDTO> Members = null!;
    }
}
