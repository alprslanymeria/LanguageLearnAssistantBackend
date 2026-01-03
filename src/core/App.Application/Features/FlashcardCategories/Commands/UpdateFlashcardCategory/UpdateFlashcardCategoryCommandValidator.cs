using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Commands.UpdateFlashcardCategory;

/// <summary>
/// VALIDATOR FOR UPDATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class UpdateFlashcardCategoryCommandValidator : AbstractValidator<UpdateFlashcardCategoryCommand>
{
    public UpdateFlashcardCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");

        RuleFor(x => x.FlashcardId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage("LANGUAGE ID MUST BE GREATER THAN 0");
    }
}
