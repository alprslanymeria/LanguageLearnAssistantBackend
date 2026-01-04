using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Commands.CreateFlashcardCategory;

/// <summary>
/// VALIDATOR FOR CREATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class CreateFlashcardCategoryCommandValidator : AbstractValidator<CreateFlashcardCategoryCommand>
{
    public CreateFlashcardCategoryCommandValidator()
    {
        RuleFor(x => x.Request.FlashcardId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.Request.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Request.LanguageId)
            .GreaterThan(0)
            .WithMessage("LANGUAGE ID MUST BE GREATER THAN 0");
    }
}
