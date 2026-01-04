using FluentValidation;

namespace App.Application.Features.FlashcardOldSessions.Commands.CreateFOS;

public class CreateFOSCommandValidator : AbstractValidator<CreateFOSCommand>
{
    public CreateFOSCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .NotEmpty()
            .WithMessage("ID IS REQUIRED");

        RuleFor(x => x.Request.FlashcardId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Request.FlashcardCategoryId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD CATEGORY ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Request.Rate)
            .InclusiveBetween(0, 100)
            .WithMessage("RATE MUST BE BETWEEN 0 AND 100.");
    }
}
