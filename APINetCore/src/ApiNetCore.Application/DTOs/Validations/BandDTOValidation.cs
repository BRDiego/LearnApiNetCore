using ApiNetCore.Application.DTOs;
using FluentValidation;

namespace ApiNetCore.Business.Models.Validations
{
    
    public class BandDTOValidation : AbstractValidator<BandDTO>
    {
        public BandDTOValidation()
        {            
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(1, 20).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");

            RuleFor(b => b.ImageFileName)
                .Length(0,100).WithMessage("The field {PropertyName} can have a maximum of {MaxLength} characters");

            RuleFor(b => b.MusicalStyles)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(1, 50).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");
        }
    }
}