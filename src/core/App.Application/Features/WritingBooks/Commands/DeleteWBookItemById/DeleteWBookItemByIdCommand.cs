using App.Application.Common;
using App.Application.Common.CQRS;

namespace App.Application.Features.WritingBooks.Commands.DeleteWBookItemById;

/// <summary>
/// COMMAND FOR DELETING A WRITING BOOK BY ID.
/// </summary>
public record DeleteWBookItemByIdCommand(int Id) : ICommand<ServiceResult>;
