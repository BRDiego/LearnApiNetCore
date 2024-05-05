using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;
using AutoMapper;

namespace ApiNetCore.Application.Configurations.MappingConfig
{
    public class BandMembersMapper : IBandMembersMapper
    {
        private Mapper customMapper;

        public BandMembersMapper()
        {
            customMapper = new Mapper(
                new MapperConfiguration(opt =>
                {
                    opt.CreateMap<Band, BandDTO>();
                    opt.CreateMap<Musician, MusicianDTO>();
                    opt.CreateMap<BandMembers, BandMembersDTO>()
                                                .ForMember(dest => dest.Band,
                                                opt => opt.MapFrom(src => src.Band))
                                                .ForPath(dest => dest.Members,
                                                opt => opt.MapFrom(src => src.Members));
                }));
        }
        public BandMembersDTO ToDto(BandMembers entity)
        {
            return customMapper.Map<BandMembersDTO>(entity);
        }

        public IEnumerable<BandMembersDTO> ToDto(IEnumerable<BandMembers> list)
        {
            var outList = Enumerable.Empty<BandMembersDTO>().ToList();

            foreach (var element in list)
                outList.Add(customMapper.Map<BandMembersDTO>(element));

            return outList;
        }
    }
}
