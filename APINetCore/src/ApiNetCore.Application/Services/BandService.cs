using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;

namespace ApiNetCore.Application.Services
{
    public class BandService : EntityService<BandDTO, Band>, IBandService
    {
        private readonly IBandRepository bandRepository;

        public BandService(IBandRepository bandRepository, 
                            IAlertManager alertManager,
                            IMapper mapper) 
                            : base(alertManager, mapper, bandRepository)
        {
            this.bandRepository = bandRepository;
        }

        public async Task<IEnumerable<BandDTO>> ListBandsByMusician(ushort musicianId)
        {
            return MapToDto(await bandRepository.ListBandsByMusician(musicianId));
        }

        public async Task<BandDTO> GetBandMembers(ushort id)
        {
            return MapToDto(await bandRepository.GetBandMembers(id));
        }
    }
}