using AutoMapper;
using ApiNetCore.Business.Models;
using ApiNetCore.Application.DTOs.Models;

namespace ApiNetCore.Application.Configurations.MappingConfig
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Band, BandDTO>().ReverseMap();
            CreateMap<Musician, MusicianDTO>().ReverseMap();
        }
    }
}