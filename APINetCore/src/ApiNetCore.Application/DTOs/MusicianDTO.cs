using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.Validations.EntityFluentValidators;
using FluentValidation;

namespace ApiNetCore.Application.DTOs
{
    public class MusicianDTO : EntityDTO, IValidDtoEntity<MusicianDTO>
    {
        public string Name { get; set; } = "";
        public string Surnames { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string PictureFileName { get; set; } = "";
        public string Roles { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        
        public List<BandDTO> Bands { get; set; } = new List<BandDTO>();

        public AbstractValidator<MusicianDTO> GetFluentValidator()
        {
            return new MusicianDTOValidation();
        }
    }
}