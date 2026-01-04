using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Commands.UpdateFlashcardCategory;

/// <summary>
/// VALIDATOR FOR UPDATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class UpdateFlashcardCategoryCommandValidator : AbstractValidator<UpdateFlashcardCategoryCommand>
{
    public UpdateFlashcardCategoryCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");

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
