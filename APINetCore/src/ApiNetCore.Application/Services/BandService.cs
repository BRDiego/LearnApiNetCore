using ApiNetCore.Application.CustomExceptions;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.DTOs.Interfaces;
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
                            IMapper mapper,
                            IBusinessRules businessRules)
                            : base(alertManager, mapper, bandRepository, businessRules)
        {
            this.bandRepository = bandRepository;
        }

        public async Task<IEnumerable<BandDTO>> ListBandsByMusician(ushort musicianId)
        {
            return MapToDto(await bandRepository.ListBandsByMusician(musicianId));
        }

        public async Task<BandDTO> GetBandWithMembers(ushort id)
        {
            return MapToDto(await bandRepository.GetBandWithMembers(id));
        }

        public async Task<IEnumerable<BandDTO>> ListByMusiciansAgeAsync(int minimumMusicianAge, int maximumMusicianAge)
        {
            if (minimumMusicianAge > 0 && !businessRules.IsValidMusicianAge(minimumMusicianAge))
                Alert("Invalid minimum age provided for filtering");

            if (maximumMusicianAge > 0 && !businessRules.IsValidMusicianAge(maximumMusicianAge))
                Alert("Invalid maximum age provided for filtering");

            if (alertManager.HasAlerts)
                InvalidRequestValueException.AlertValidationException();

            var minBirthYear = DateTime.Now.Year - minimumMusicianAge;
            var maxBirthYear = DateTime.Now.Year - maximumMusicianAge;

            IEnumerable<Band> result;

            if (minimumMusicianAge > 0 && maximumMusicianAge > 0)
                result = await bandRepository.ListAsync(
                    b => b.Musicians.Any(mus => mus.DateOfBirth.Year >= minBirthYear && mus.DateOfBirth.Year <= maxBirthYear));
            else if (minimumMusicianAge > 0)
                result = await bandRepository.ListAsync(
                    b => b.Musicians.Any(mus => mus.DateOfBirth.Year >= minBirthYear));
            else if (maximumMusicianAge > 0)
                result = await bandRepository.ListAsync(
                    b => b.Musicians.Any(mus => mus.DateOfBirth.Year <= maxBirthYear));
            else
                result = await bandRepository.ListAsync();

            return MapToDto(result);
        }

        public async Task<BandDTO> FindByNameAsync(string name)
        {
            if (!businessRules.IsValidBandName(name))
            {
                Alert("Invalid name parameter");
                InvalidRequestValueException.AlertValidationException();
            }

            return MapToDto(await bandRepository.FindAsync(b => b.Name.Contains(name)));
        }
    }
}