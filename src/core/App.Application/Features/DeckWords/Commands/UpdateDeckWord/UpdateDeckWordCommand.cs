using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords.Commands.UpdateDeckWord;

/// <summary>
/// COMMAND FOR UPDATING AN EXISTING DECK WORD.
/// </summary>
public record UpdateDeckWordCommand(UpdateDeckWordRequest Request) : ICommand<ServiceResult>;
