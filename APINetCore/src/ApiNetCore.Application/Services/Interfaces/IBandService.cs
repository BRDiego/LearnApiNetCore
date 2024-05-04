using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IBandService: IEntityService<BandDTO, Band>, IEntityImageService
    {
        Task<BandDTO?> FindByNameAsync(string? name);
    }
}