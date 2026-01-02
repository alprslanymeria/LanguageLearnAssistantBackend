using App.Application.Features.WritingSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.WritingSessionRows.Validators;

/// <summary>
/// VALIDATOR FOR SAVE WRITING ROWS REQUEST.
/// </summary>
public class SaveWritingRowsRequestValidator : AbstractValidator<SaveWritingRowsRequest>
{
    public SaveWritingRowsRequestValidator()
    {
        RuleFor(x => x.WritingOldSessionId)
            .NotEmpty()
            .WithMessage("WRITING OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Rows).SetValidator(new WritingRowItemRequestValidator());
    }
}

/// <summary>
/// VALIDATOR FOR WRITING ROW ITEM REQUEST.
/// </summary>
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
