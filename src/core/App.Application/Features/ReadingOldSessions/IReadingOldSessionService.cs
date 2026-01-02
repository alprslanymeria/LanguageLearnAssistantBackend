using App.Application.Common;
using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.ReadingOldSessions;

/// <summary>
/// SERVICE INTERFACE FOR READING OLD SESSION OPERATIONS.
/// </summary>
public interface IReadingOldSessionService
{
    /// <summary>
    /// RETRIEVES ALL READING OLD SESSIONS FOR A USER.
    /// </summary>
    Task<ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>> GetReadingOldSessionsWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// SAVES A NEW READING OLD SESSION.
    /// </summary>
    Task<ServiceResult<ReadingOldSessionDto>> SaveReadingOldSessionAsync(SaveReadingOldSessionRequest request);
}
