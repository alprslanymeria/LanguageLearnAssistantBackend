using App.Application.Features.Writings.Dtos;
using FluentValidation;

namespace App.Application.Features.Writings.Validators;

public class CreateWritingDtoValidator : AbstractValidator<CreateWritingDto>
{
    public CreateWritingDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language Id must be greater than 0");

        RuleFor(x => x.PracticeId)
            .GreaterThan(0).WithMessage("Practice Id must be greater than 0");
    }
}
