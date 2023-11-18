using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Business.Models.Validations;
using ApiNetCore.Business.Services.Interfaces;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;

namespace ApiNetCore.Business.Services
{
    public class BandService : EntityBaseService<Band>, IBandService
    {
        private readonly IBandRepository bandRepository;

        public BandService(IBandRepository bandRepository, 
                            IAlertManager alertManager,
                            IMapper mapper) : base(alertManager, mapper)
        {
            this.bandRepository = bandRepository;
        }
        
        public async Task Add(BandDTO band)
        {
            if (!ExecuteValidation(new BandDTOValidation(), band)) return;

            await bandRepository.AddAsync(Map(band));
        }

        public async Task Update(BandDTO band)
        {
            if (!ExecuteValidation(new BandDTOValidation(), band )) return;

            await bandRepository.UpdateAsync(Map(band));
        }

        public async Task Delete(ushort id)
        {
            await bandRepository.DeleteAsync(id);
        }

        public void Dispose()
        {
            bandRepository?.Dispose();
        }
    }
}