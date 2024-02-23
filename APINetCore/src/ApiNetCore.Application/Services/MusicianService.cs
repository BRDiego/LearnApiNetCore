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

        public async Task<IEnumerable<MusicianDTO>> ListMusiciansByBand(ushort bandId)
        {
            return MapToDto(await musicianRepository.ListMusiciansByBand(bandId));
        }

        public async Task<MusicianDTO> GetMusicianWithBands(ushort id)
        {
            return MapToDto(await musicianRepository.GetMusicianWithBands(id));
        }

        public async Task<IEnumerable<MusicianDTO>> SearchAsync(int musicianAge, string surname)
        {
            businessRules.ValidateMusicianAge(musicianAge);
            businessRules.ValidateMusicianSurname(surname);

            alertManager.CheckAlerts();

            return MapToDto(await musicianRepository.ListAsync(m =>
                    musicianAge > 0 ? m.Age == musicianAge : true
                    &&
                    m.Surnames.Contains(surname)
                    ));
        }

        public async Task<IEnumerable<MusicianDTO>> ListByNicknameAsync(string nickname)
        {
            businessRules.ValidateMusicianNickname(nickname);
            alertManager.CheckAlerts();

            return MapToDto(await musicianRepository.ListAsync(m => m.Nickname.Contains(nickname)));
        }
    }
}