using ApiNetCore.Application.DTOs.Validations;
using FluentValidation;

namespace ApiNetCore.Application.DTOs
{
    public class MusicianDTO : EntityDTO
    {
        public string Name { get; set; } = "";
        public string Surnames { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string PictureFileName { get; set; } = "";
        public string Roles { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        
        public List<BandDTO> Bands { get; set; } = new List<BandDTO>();
    }
}