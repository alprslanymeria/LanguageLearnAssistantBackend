using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.ReadingOldSessions.Commands.SaveReadingOldSession;

/// <summary>
/// COMMAND FOR SAVING A NEW READING OLD SESSION.
/// </summary>
public record SaveReadingOldSessionCommand(
    string Id,
    int ReadingId,
    int ReadingBookId,
    decimal Rate
    ) : ICommand<ServiceResult<ReadingOldSessionDto>>;
