using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Commands;

/// <summary>
/// VALIDATOR FOR DELETE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class DeleteFlashcardCategoryCommandValidator : AbstractValidator<DeleteFlashcardCategoryCommand>
{
    public DeleteFlashcardCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID MUST BE GREATER THAN 0");
    }
}
