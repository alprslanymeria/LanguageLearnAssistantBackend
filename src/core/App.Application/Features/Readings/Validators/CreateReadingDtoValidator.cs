using App.Application.Features.Readings.Dtos;
using FluentValidation;

namespace App.Application.Features.Readings.Validators;

public class CreateReadingDtoValidator : AbstractValidator<CreateReadingDto>
{
    public CreateReadingDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language Id must be greater than 0");

        RuleFor(x => x.PracticeId)
            .GreaterThan(0).WithMessage("Practice Id must be greater than 0");
    }
}
