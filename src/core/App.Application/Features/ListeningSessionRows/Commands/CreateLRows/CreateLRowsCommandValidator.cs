using App.Application.Features.ListeningSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.ListeningSessionRows.Commands.CreateLRows;

public class CreateLRowsCommandValidator : AbstractValidator<CreateLRowsCommand>
{
    public CreateLRowsCommandValidator()
    {
        RuleFor(x => x.Request.ListeningOldSessionId)
            .NotEmpty()
            .WithMessage("LISTENING OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Request.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Request.Rows).SetValidator(new ListeningRowItemRequestValidator());
    }
}

public class ListeningRowItemRequestValidator : AbstractValidator<ListeningRowItemRequest>
{
    public ListeningRowItemRequestValidator()
    {
        RuleFor(x => x.ListenedSentence)
            .NotEmpty()
            .WithMessage("LISTENED SENTENCE IS REQUIRED.")
            .MaximumLength(2000)
            .WithMessage("LISTENED SENTENCE MUST NOT EXCEED 2000 CHARACTERS.");

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED.")
            .MaximumLength(2000)
            .WithMessage("ANSWER MUST NOT EXCEED 2000 CHARACTERS.");

        RuleFor(x => x.Similarity)
            .InclusiveBetween(0, 100)
            .WithMessage("SIMILARITY MUST BE BETWEEN 0 AND 100.");
    }
}
