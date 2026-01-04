using FluentValidation;

namespace App.Application.Features.ListeningOldSessions.Commands.CreateLOS;

public class CreateLOSCommandValidator : AbstractValidator<CreateLOSCommand>
{
    public CreateLOSCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.Request.ListeningId)
            .GreaterThan(0)
            .WithMessage("LISTENING ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Request.ListeningCategoryId)
            .GreaterThan(0)
            .WithMessage("LISTENING CATEGORY ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Request.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100.");
    }
}
