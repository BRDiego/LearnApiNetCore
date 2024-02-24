using ApiNetCore.Application.CustomExceptions;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;

namespace ApiNetCore.Application.Services
{
    public class MusicianService : EntityService<MusicianDTO, Musician>, IMusicianService
    {
        private readonly IMusicianRepository musicianRepository;

        public MusicianService(IMusicianRepository musicianRepository,
                            IAlertManager alertManager,
                            IMapper mapper,
                            IBusinessRules businessRules)
                            : base(alertManager, mapper, musicianRepository, businessRules)
        {
            this.musicianRepository = musicianRepository;
        }

        public async Task<MusicianDTO> GetMusicianWithBands(ushort id)
        {
            return MapToDto(await musicianRepository.GetMusicianWithBands(id));
        }

        public async Task<IEnumerable<MusicianDTO>> SearchAsync(int? musicianAge, string? surname, string? nickname)
        {
            businessRules.ValidateMusicianAge(ref musicianAge);
            businessRules.ValidateMusicianSurname(ref surname);
            businessRules.ValidateMusicianNickname(ref nickname);

            alertManager.CheckAlerts();

            var possibleYearsOfBirth = new List<int>();
            var yearOfBirth = DateTime.Now.Year - musicianAge!.Value;

            possibleYearsOfBirth.Add(yearOfBirth);
            possibleYearsOfBirth.Add(yearOfBirth - 1);

            return MapToDto(await musicianRepository.ListAsync(m =>
                    
                    musicianAge > 0 ? possibleYearsOfBirth.Contains(m.DateOfBirth.Year) : true
                    &&
                    m.Surnames.Contains(surname!)
                    &&
                    m.Nickname.Contains(nickname!)
                    ));
        }

        protected async override Task<bool> PassesDuplicityCheck(Musician entityModel)
        {
            var register = await musicianRepository.FindAsync(
                m => m.Name == entityModel.Name
                &&
                m.DateOfBirth == entityModel.DateOfBirth
                );

            return register is null;
        }
    }
}