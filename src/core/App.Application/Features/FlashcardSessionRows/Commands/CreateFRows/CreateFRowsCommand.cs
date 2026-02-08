using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardSessionRows.Dtos;

namespace App.Application.Features.FlashcardSessionRows.Commands.CreateFRows;

public record CreateFRowsCommand(SaveFlashcardRowsRequest Request) : ICommand<ServiceResult>;
