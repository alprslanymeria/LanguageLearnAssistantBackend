using App.Application.Features.ReadingSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.ReadingSessionRows.Commands.CreateRRows;

public class CreateRRowsCommandValidator : AbstractValidator<CreateRRowsCommand>
{
    public CreateRRowsCommandValidator()
    {
        RuleFor(x => x.Request.ReadingOldSessionId)
            .NotEmpty()
            .WithMessage("READING OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Request.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Request.Rows).SetValidator(new ReadingRowItemRequestValidator());
    }
}

public class ReadingRowItemRequestValidator : AbstractValidator<ReadingRowItemRequest>
{
    public ReadingRowItemRequestValidator()
    {
        RuleFor(x => x.SelectedSentence)
            .NotEmpty()
            .WithMessage("SELECTED SENTENCE IS REQUIRED.")
            .MaximumLength(2000)
            .WithMessage("SELECTED SENTENCE MUST NOT EXCEED 2000 CHARACTERS.");

        RuleFor(x => x.Answer)
            .NotEmpty()
            .WithMessage("ANSWER IS REQUIRED.")
            .MaximumLength(2000)
            .WithMessage("ANSWER MUST NOT EXCEED 2000 CHARACTERS.");

        RuleFor(x => x.AnswerTranslate)
            .NotEmpty()
            .WithMessage("ANSWER TRANSLATE IS REQUIRED.")
            .MaximumLength(2000)
            .WithMessage("ANSWER TRANSLATE MUST NOT EXCEED 2000 CHARACTERS.");

        RuleFor(x => x.Similarity)
            .InclusiveBetween(0, 100)
            .WithMessage("SIMILARITY MUST BE BETWEEN 0 AND 100.");
    }
}
