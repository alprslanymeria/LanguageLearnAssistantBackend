using FluentValidation;

namespace App.Application.Features.WritingBooks.Commands.DeleteWBookItemById;

/// <summary>
/// VALIDATOR FOR DELETE WRITING BOOK BY ID COMMAND.
/// </summary>
public class DeleteWBookItemByIdCommandValidator : AbstractValidator<DeleteWBookItemByIdCommand>
{
    public DeleteWBookItemByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
