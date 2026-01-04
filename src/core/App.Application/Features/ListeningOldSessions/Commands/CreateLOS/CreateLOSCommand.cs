using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ListeningOldSessions.Dtos;

namespace App.Application.Features.ListeningOldSessions.Commands.CreateLOS;

public record CreateLOSCommand(SaveListeningOldSessionRequest Request) : ICommand<ServiceResult<string>>;
