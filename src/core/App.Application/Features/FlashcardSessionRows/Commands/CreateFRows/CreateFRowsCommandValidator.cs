using App.Application.Features.FlashcardSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.FlashcardSessionRows.Commands.CreateFRows;

public class CreateFRowsCommandValidator : AbstractValidator<CreateFRowsCommand>
{
    public CreateFRowsCommandValidator()
    {
        RuleFor(x => x.Request.FlashcardOldSessionId)
            .NotEmpty()
            .WithMessage("FLASHCARD OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Request.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Request.Rows).SetValidator(new FlashcardRowItemRequestValidator());
    }
}

public class FlashcardRowItemRequestValidator : AbstractValidator<FlashcardRowItemRequest>
{
    public FlashcardRowItemRequestValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("QUESTION IS REQUIRED.")
            .MaximumLength(500)
            .WithMessage("QUESTION MUST NOT EXCEED 500 CHARACTERS.");

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED.")
            .MaximumLength(500)
            .WithMessage("ANSWER MUST NOT EXCEED 500 CHARACTERS.");
    }
}
