using FluentValidation;

namespace App.Application.Features.ReadingBooks.Commands.DeleteRBookItemById;

/// <summary>
/// VALIDATOR FOR DELETE READING BOOK BY ID COMMAND.
/// </summary>
public class DeleteRBookItemByIdCommandValidator : AbstractValidator<DeleteRBookItemByIdCommand>
{
    public DeleteRBookItemByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");
    }
}
