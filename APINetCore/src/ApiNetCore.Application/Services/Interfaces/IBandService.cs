using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IBandService: IEntityService<BandDTO, Band>
    {
        Task<IEnumerable<BandDTO>> ListBandsByMusicianId(ushort musicianId);
        Task<BandDTO> GetBandWithMembers(ushort id);
        Task<IEnumerable<BandDTO>> ListByMusiciansAgeAsync(int minimumMusicianAge, int maximumMusicianAge);
        Task<BandDTO> FindByNameAsync(string name);
    }
}