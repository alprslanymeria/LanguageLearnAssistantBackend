using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Commands.DeleteFCategoryItemById;

/// <summary>
/// VALIDATOR FOR DELETE FLASHCARD CATEGORY BY ID COMMAND.
/// </summary>
public class DeleteFCategoryItemByIdCommandValidator : AbstractValidator<DeleteFCategoryItemByIdCommand>
{
    public DeleteFCategoryItemByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
