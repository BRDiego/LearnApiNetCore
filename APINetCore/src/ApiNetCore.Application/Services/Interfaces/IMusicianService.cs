using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Business.Services.Interfaces
{
    public interface IMusicianService: IDisposable
    {
        Task Add(MusicianDTO musician);
        Task Update(MusicianDTO musician);
        Task Delete(ushort id);
    }
}