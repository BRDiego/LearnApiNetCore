using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;
using Microsoft.AspNetCore.Http;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IBandService: IEntityService<BandDTO, Band>
    {
        Task<BandDTO> GetBandWithMembers(ushort id);
        Task<IEnumerable<BandDTO>> ListByMusiciansAgeAsync(int? minimumMusicianAge, int? maximumMusicianAge);
        Task<BandDTO?> FindByNameAsync(string? name);
        Task UpdateImageAsync(ushort id, IFormFile imageUpload);
    }
}