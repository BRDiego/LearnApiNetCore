using ApiNetCore.Application.DTOs.Models;
using FluentValidation;
namespace ApiNetCore.Application.DTOs.Validations.EntityFluentValidators
{
    internal class BandImageDTOValidation : AbstractValidator<BandImageDTO>
    {
        public BandImageDTOValidation()
        {
            RuleFor(b => b.ImageUploadingBase64)
                .Empty().When(b => b.ImageUploadStream is not null)
                .WithMessage("only one image transfer option can be used");
        }
    }
}
