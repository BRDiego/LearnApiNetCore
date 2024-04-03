using AutoMapper;
using ApiNetCore.Business.Models;
using ApiNetCore.Application.DTOs.Models;

namespace DevIO.Api.Configuration
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