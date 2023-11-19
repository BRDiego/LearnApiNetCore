using ApiNetCore.Application.DTOs;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IMusicianService: IDisposable
    {
        Task<IEnumerable<MusicianDTO>> ListMusiciansByBand(ushort bandId);
        Task<MusicianDTO> GetMusicianBands(ushort id);
    }
}