using App.Application.Features.WritingSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.WritingSessionRows.Commands.CreateWRows;

public class CreateWRowsCommandValidator : AbstractValidator<CreateWRowsCommand>
{
    public CreateWRowsCommandValidator()
    {
        RuleFor(x => x.Request.WritingOldSessionId)
            .NotEmpty()
            .WithMessage("WRITING OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Request.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Request.Rows).SetValidator(new WritingRowItemRequestValidator());
    }
}

public class WritingRowItemRequestValidator : AbstractValidator<WritingRowItemRequest>
{
    public WritingRowItemRequestValidator()
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
