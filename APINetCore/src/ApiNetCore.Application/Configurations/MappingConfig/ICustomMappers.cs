using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Application.Configurations.MappingConfig
{
    public interface IBandMembersMapper
    {
        BandMembersDTO ToDto(BandMembers entity);
        IEnumerable<BandMembersDTO> ToDto(IEnumerable<BandMembers> entity);
    }

    public interface IMusicianMembershipsMapper
    {
        MusicianMembershipsDTO ToDto(MusicianMemberships entity);
        IEnumerable<MusicianMembershipsDTO> ToDto(IEnumerable<MusicianMemberships> entity);
    }
}
