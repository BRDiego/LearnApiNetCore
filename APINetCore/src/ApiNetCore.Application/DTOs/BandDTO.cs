using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Application.DTOs.Validations.EntityFluentValidators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ApiNetCore.Application.DTOs
{
    public class BandDTO : EntityDTO, IValidDtoEntity<BandDTO>
    {
        public string Name { get; set; } = "";
        public string MusicalStyles { get; set; } = "";
        public string ImageFileName { get; set; } = "";
    
        public List<MusicianDTO> Musicians { get; set; } = new List<MusicianDTO>();

        public string ImageUploadingBase64 { get; set; } = "";
        public string ImageUploadingName { get; set; } = "";
        
        [JsonIgnore]
        public IFormFile? ImageUploadStream { get; set; }
        public AbstractValidator<BandDTO> GetFluentValidator()
        {
            return new BandDTOValidation();
        }
    }
}