using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;
using AutoMapper;

namespace ApiNetCore.Application.DTOs.MappingConfig
{
    public class MusicianMembershipMapper : IMusicianMembershipsMapper
    {
        private Mapper customMapper;

        public MusicianMembershipMapper()
        {
            customMapper = new Mapper(
                new MapperConfiguration(opt =>
                                        opt.CreateMap<MusicianMemberships, MusicianMembershipsDTO>()
                                        .ForMember(dest => dest,
                                        src => src.MapFrom(src => src.Musician))
                                        .ForMember(dest => dest.Bands,
                                        src => src.MapFrom(src => src.Memberships))
                                        )
                );
        }

        public MusicianMembershipsDTO ToDto(MusicianMemberships entity)
        {
            return customMapper.Map<MusicianMembershipsDTO>(entity);
        }

        public IEnumerable<MusicianMembershipsDTO> ToDto(IEnumerable<MusicianMemberships> list)
        {
            var outList = Enumerable.Empty<MusicianMembershipsDTO>().ToList();

            foreach (var element in list)
                outList.Add(customMapper.Map<MusicianMembershipsDTO>(element));
                

            return outList;
        }
    }
}
