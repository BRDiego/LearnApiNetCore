using ApiNetCore.Application.DTOs;
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
                            IMapper mapper) 
                            : base(alertManager, mapper, musicianRepository)
        {
            this.musicianRepository = musicianRepository;
        }

        public async Task<IEnumerable<MusicianDTO>> ListMusiciansByBand(ushort bandId)
        {
            return MapToDto(await musicianRepository.ListMusiciansByBand(bandId));
        }
        
        public async Task<MusicianDTO> GetMusicianBands(ushort id)
        {
            return MapToDto(await musicianRepository.GetMusicianBands(id));
        }
    }
}