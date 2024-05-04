using ApiNetCore.Application.DTOs.Models;
using ApiNetCore.Business.Models;
using AutoMapper;

namespace ApiNetCore.Application.DTOs.MappingConfig
{
    public class BandMembersMapper : IBandMembersMapper
    {
        private Mapper customMapper;

        public BandMembersMapper()
        {
            customMapper = new Mapper(
                new MapperConfiguration(opt =>
                {
                    opt.CreateMap<Band, BandDTO>(); // Map Band to BandDTO automatically
                    opt.CreateMap<Musician, MusicianDTO>(); // Map Band to BandDTO automatically
                    opt.CreateMap<BandMembers, BandMembersDTO>()
                                                .ForMember(dest => dest,
                                                opt => opt.MapFrom(src => src.Band))
                                                .ForPath(dest => dest.Musicians,
                                                opt => opt.MapFrom(src => src.Members));
                }));

            //customMapper = new Mapper(
            //                    new MapperConfiguration(opt =>
            //                    {
            //                        //opt.CreateMap<Band, BandDTO>(); // Map Band to BandDTO automatically

            //                        opt.CreateMap<BandMembers, BandMembersDTO>()
            //                        .ForMember(dest => dest.Musicians,
            //                                    opt => opt.MapFrom(src => src.Members))
            //                        .IncludeBase<Band, BandDTO>(); // Include mapping from Band to BandDTO
            //                        })
            //                    );
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
