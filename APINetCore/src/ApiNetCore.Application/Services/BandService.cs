using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Business.Services.Interfaces;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;

namespace ApiNetCore.Business.Services
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
    }
}