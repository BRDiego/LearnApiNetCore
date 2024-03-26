using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;
using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IBandService: IEntityService<BandDTO, Band>, IEntityImageService
    {
        Task<BandDTO> GetBandWithMembers(ushort id);
        Task<IEnumerable<BandDTO>> ListByMusiciansAgeAsync(int? minimumMusicianAge, int? maximumMusicianAge);
        Task<BandDTO?> FindByNameAsync(string? name);
    }
}