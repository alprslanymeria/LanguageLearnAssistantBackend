using App.Application.Features.Listenings.Dtos;
using FluentValidation;

namespace App.Application.Features.Listenings.Validators;

public class UpdateListeningDtoValidator : AbstractValidator<UpdateListeningDto>
{
    public UpdateListeningDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Listening Id must be greater than 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language Id must be greater than 0");

        RuleFor(x => x.PracticeId)
            .GreaterThan(0).WithMessage("Practice Id must be greater than 0");
    }
}
