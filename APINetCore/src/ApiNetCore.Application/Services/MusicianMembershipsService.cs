using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.MappingConfig;
using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Data.EFContext.Repository.Interfaces;

namespace ApiNetCore.Application.Services
{
    public class MusicianMembershipsService : BaseService, IMusicianMembershipsService
    {
        private readonly IMusicianMembershipsMapper mapper;
        private readonly IMusicianRepository repo;
        private readonly IBusinessRules rules;

        public MusicianMembershipsService(IAlertManager alertManager, IMusicianRepository repo, IBusinessRules rules, IMusicianMembershipsMapper mapper) : base(alertManager)
        {
            this.repo = repo;
            this.rules = rules;
            this.mapper = mapper;            
        }

        public async Task<MusicianMembershipsDTO> GetMusicianWithBands(ushort id)
        {
            return mapper.ToDto(await repo.GetMusicianWithBands(id));
        }
    }
}
