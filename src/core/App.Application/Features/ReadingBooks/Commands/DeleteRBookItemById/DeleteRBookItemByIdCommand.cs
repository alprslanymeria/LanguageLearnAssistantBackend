using App.Application.Common;
using App.Application.Common.CQRS;

namespace App.Application.Features.ReadingBooks.Commands.DeleteRBookItemById;

/// <summary>
/// COMMAND FOR DELETING A READING BOOK BY ID.
/// </summary>
public record DeleteRBookItemByIdCommand(int Id) : ICommand<ServiceResult>;
