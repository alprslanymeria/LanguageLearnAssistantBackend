using FluentValidation;

namespace App.Application.Features.DeckWords.Commands.DeleteDWordItemById;

/// <summary>
/// VALIDATOR FOR DELETE DECK WORD BY ID COMMAND.
/// </summary>
public class DeleteDWordItemByIdCommandValidator : AbstractValidator<DeleteDWordItemByIdCommand>
{
    public DeleteDWordItemByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
