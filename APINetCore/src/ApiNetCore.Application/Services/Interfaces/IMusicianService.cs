using ApiNetCore.Application.DTOs;

namespace ApiNetCore.Business.Services.Interfaces
{
    public interface IMusicianService: IDisposable
    {
        Task<IEnumerable<MusicianDTO>> ListMusiciansByBand(ushort bandId);
    }
}