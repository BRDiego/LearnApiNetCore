using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IBandService: IEntityService<BandDTO, Band>
    {
        Task<IEnumerable<BandDTO>> ListBandsByMusician(ushort musicianId);
        Task<BandDTO> GetBandMembers(ushort id);
    }
}