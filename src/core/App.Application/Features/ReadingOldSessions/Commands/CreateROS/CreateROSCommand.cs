using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.ReadingOldSessions.Commands.CreateROS;

/// <summary>
/// COMMAND FOR SAVING A NEW READING OLD SESSION.
/// </summary>
public record CreateROSCommand(SaveReadingOldSessionRequest Request) : ICommand<ServiceResult>;
