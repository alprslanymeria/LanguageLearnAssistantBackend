using App.Application.Common;
using App.Application.Common.CQRS;

namespace App.Application.Features.DeckWords.Commands.DeleteDWordItemById;

/// <summary>
/// COMMAND FOR DELETING A DECK WORD BY ID.
/// </summary>
public record DeleteDWordItemByIdCommand(int Id) : ICommand<ServiceResult>;
