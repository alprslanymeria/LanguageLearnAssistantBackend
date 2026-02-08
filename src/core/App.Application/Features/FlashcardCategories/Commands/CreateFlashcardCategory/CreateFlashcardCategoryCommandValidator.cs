using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Commands.CreateFlashcardCategory;

/// <summary>
/// VALIDATOR FOR CREATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class CreateFlashcardCategoryCommandValidator : AbstractValidator<CreateFlashcardCategoryCommand>
{
    public CreateFlashcardCategoryCommandValidator()
    {

        RuleFor(x => x.Request.CategoryName)
            .NotEmpty()
            .WithMessage("CATEGOY NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("CATEGORY NAME MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.Request.Practice)
            .NotEmpty()
            .WithMessage("PRACTICE IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("PRACTICE MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.Request.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Request.LanguageId)
            .GreaterThan(0)
            .WithMessage("LANGUAGE ID MUST BE GREATER THAN 0");
    }
}
