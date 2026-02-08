using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingOldSessions.Dtos;

namespace App.Application.Features.WritingOldSessions.Commands.CreateWOS;

public record CreateWOSCommand(SaveWritingOldSessionRequest Request) : ICommand<ServiceResult>;
