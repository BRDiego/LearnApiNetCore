using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IMusicianService: IEntityService<MusicianDTO, Musician>, IEntityImageService
    {
        Task<IEnumerable<MusicianDTO>> SearchAsync(int? musicianAge, string? surname, string? nickname);
    }
}