using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords.Commands.CreateDeckWord;

/// <summary>
/// COMMAND FOR CREATING A NEW DECK WORD.
/// </summary>
public record CreateDeckWordCommand(CreateDeckWordRequest Request) : ICommand<ServiceResult>;
