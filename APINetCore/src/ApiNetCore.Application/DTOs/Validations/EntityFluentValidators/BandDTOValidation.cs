using FluentValidation;
using System.Text.RegularExpressions;

namespace ApiNetCore.Application.DTOs.Validations.EntityFluentValidators
{
    public partial class BandDTOValidation : AbstractValidator<BandDTO>
    {
        public BandDTOValidation()
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(1, 20).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");

            RuleFor(b => b.MusicalStyles)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(3, 50).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");
            
            RuleFor(b => b.ImageUploadingName)
                .NotEmpty().When(b => !string.IsNullOrEmpty(b.ImageUploadingBase64))
                .WithMessage("image name must be filled when sending a base64 image")
                .Must(name => ImageNameHasExtensionRegex().IsMatch(name)).When(b => !string.IsNullOrEmpty(b.ImageUploadingName))
                .WithMessage("image extension could not be identified")
                .Must(name => ValidImageExtensionRegex().IsMatch(name)).When(b => !string.IsNullOrEmpty(b.ImageUploadingName))
                .WithMessage("only jpg, jpeg and png extensions are allowed");
        }


        [GeneratedRegex("\\.[a-zA-Z0-9]+$")]
        private static partial Regex ImageNameHasExtensionRegex();

        [GeneratedRegex("\\.(jpg|png|jpeg)$")]
        private static partial Regex ValidImageExtensionRegex();
    }
}