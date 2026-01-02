using App.Application.Features.ReadingSessionRows.Dtos;
using FluentValidation;

namespace App.Application.Features.ReadingSessionRows.Validators;

/// <summary>
/// VALIDATOR FOR SAVE READING ROWS REQUEST.
/// </summary>
public class SaveReadingRowsRequestValidator : AbstractValidator<SaveReadingRowsRequest>
{
    public SaveReadingRowsRequestValidator()
    {
        RuleFor(x => x.ReadingOldSessionId)
            .NotEmpty()
            .WithMessage("READING OLD SESSION ID IS REQUIRED.");

        RuleFor(x => x.Rows)
            .NotEmpty()
            .WithMessage("AT LEAST ONE ROW IS REQUIRED.");

        RuleForEach(x => x.Rows).SetValidator(new ReadingRowItemRequestValidator());
    }
}

/// <summary>
/// VALIDATOR FOR READING ROW ITEM REQUEST.
/// </summary>
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
