using ApiNetCore.Application.DTOs.Models;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IMusicianMembershipsService
    {
        Task<MusicianMembershipsDTO> GetMusicianWithBands(ushort id);
    }
}
