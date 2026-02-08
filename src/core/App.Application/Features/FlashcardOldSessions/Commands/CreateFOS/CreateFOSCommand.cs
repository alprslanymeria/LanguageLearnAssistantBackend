using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardOldSessions.Dtos;

namespace App.Application.Features.FlashcardOldSessions.Commands.CreateFOS;

public record CreateFOSCommand(SaveFlashcardOldSessionRequest Request) : ICommand<ServiceResult>;
