using FluentValidation;

namespace ApiNetCore.Application.DTOs.Validations
{
    public class MusicianDTOValidation : AbstractValidator<MusicianDTO>
    {
        public MusicianDTOValidation()
        {            
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(1, 20).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");

            RuleFor(m => m.Surnames)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(0, 50).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");

            RuleFor(b => b.PictureFileName)
                .Length(0,100).WithMessage("The field {PropertyName} can have a maximum of {MaxLength} characters");

            RuleFor(m => m.Nickname)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(1, 20).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");

            RuleFor(b => b.Roles)
                .NotEmpty().WithMessage("The field {PropertyName} must be filled")
                .Length(1, 50).WithMessage("The field {PropertyName} must contain {MinLength} to {MaxLength} characters");
                 
            RuleFor(b => b.DateOfBirth)
                .NotNull().WithMessage("The field {PropertyName} must be provided")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Only musicians with 18 years or older can be subscribed")
                .GreaterThan(new DateTime(1920, 1, 1)).WithMessage("The {PropertyName} was provided with an invalid year.");
        }
    }
}