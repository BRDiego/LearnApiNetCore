using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.Validations.EntityFluentValidators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ApiNetCore.Application.DTOs.Models
{
    public class MusicianDTO : EntityDTO, IValidDtoEntity<MusicianDTO>
    {
        public string Name { get; set; } = "";
        public string Surnames { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string PictureFileName { get; set; } = "";
        public string Roles { get; set; } = "";
        public DateTime DateOfBirth { get; set; }

        public string ImageUploadingBase64 { get; set; } = "";
        public string ImageUploadingName { get; set; } = "";

        [JsonIgnore]
        public IFormFile? ImageUploadStream { get; set; }
        public AbstractValidator<MusicianDTO> GetFluentValidator()
        {
            return new MusicianDTOValidation();
        }
    }
}