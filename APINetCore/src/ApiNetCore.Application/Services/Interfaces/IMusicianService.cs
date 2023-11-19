using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IMusicianService: IEntityService<MusicianDTO, Musician>
    {
        Task<IEnumerable<MusicianDTO>> ListMusiciansByBand(ushort bandId);
        Task<MusicianDTO> GetMusicianBands(ushort id);
    }
}