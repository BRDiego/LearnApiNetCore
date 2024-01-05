using AutoMapper;
using ApiNetCore.Application.DTOs;
using ApiNetCore.Business.Models;

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