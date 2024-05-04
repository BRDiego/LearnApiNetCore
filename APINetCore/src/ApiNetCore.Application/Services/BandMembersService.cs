using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.MappingConfig;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Models;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using AutoMapper;

namespace ApiNetCore.Application.Services
{
    public class BandMembersService : BaseService , IBandMembersService
    {
        private readonly IBandMembersMapper mapper;
        private readonly IBandRepository bandRepository;
        private readonly IBusinessRules rules;

        public BandMembersService(IAlertManager alertManager, IBandRepository bandRepository, IBusinessRules rules, IBandMembersMapper mapper) : base (alertManager)
        {
            this.bandRepository = bandRepository;
            this.rules = rules;
            this.mapper = mapper;
        }

        public async Task<BandMembersDTO> GetBandWithMembers(ushort id)
        {
            return mapper.ToDto(await bandRepository.GetBandWithMembers(id));
        }

        public async Task<IEnumerable<BandMembersDTO>> ListByMusiciansAgeAsync(int? minimumMusicianAge, int? maximumMusicianAge)
        {
            rules.ValidateMusicianAge(ref minimumMusicianAge);
            rules.ValidateMusicianAge(ref maximumMusicianAge);

            alertManager.CheckAlerts();

            var minBirthDate = minimumMusicianAge > 0 ?  DateTime.Now.AddYears(-minimumMusicianAge.Value) : DateTime.Now.AddYears(-18);
            var maxBirthDate = maximumMusicianAge > 0 ? DateTime.Now.AddYears(-maximumMusicianAge.Value) : DateTime.Now.AddYears(-100);

            var result = await bandRepository.ListByMembersDataAsync(minBirthDate, maxBirthDate);
            return mapper.ToDto(result);
        }
    }
}
