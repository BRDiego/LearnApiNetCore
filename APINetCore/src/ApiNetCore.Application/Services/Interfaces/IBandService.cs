using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Business.Services.Interfaces
{
    public interface IBandService: IDisposable
    {
        Task Add(BandDTO band);
        Task Update(BandDTO band);
        Task Delete(ushort id);
    }
}