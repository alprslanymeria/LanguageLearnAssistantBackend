using App.Application.Common;
using App.Application.Features.WritingOldSessions.Dtos;

namespace App.Application.Features.WritingOldSessions;

/// <summary>
/// SERVICE INTERFACE FOR WRITING OLD SESSION OPERATIONS.
/// </summary>
public interface IWritingOldSessionService
{
    /// <summary>
    /// RETRIEVES ALL WRITING OLD SESSIONS FOR A USER.
    /// </summary>
    Task<ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>> GetWritingOldSessionsWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// SAVES A NEW WRITING OLD SESSION.
    /// </summary>
    Task<ServiceResult<WritingOldSessionDto>> SaveWritingOldSessionAsync(SaveWritingOldSessionRequest request);
}
