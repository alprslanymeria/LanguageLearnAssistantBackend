using App.Application.Features.Writings.Dtos;
using FluentValidation;

namespace App.Application.Features.Writings.Validators;

public class UpdateWritingDtoValidator : AbstractValidator<UpdateWritingDto>
{
    public UpdateWritingDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Writing Id must be greater than 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language Id must be greater than 0");

        RuleFor(x => x.PracticeId)
            .GreaterThan(0).WithMessage("Practice Id must be greater than 0");
    }
}
