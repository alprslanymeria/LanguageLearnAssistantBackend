using App.Application.Common;
using App.Application.Features.ListeningOldSessions.Dtos;

namespace App.Application.Features.ListeningOldSessions;

/// <summary>
/// SERVICE INTERFACE FOR LISTENING OLD SESSION OPERATIONS.
/// </summary>
public interface IListeningOldSessionService
{
    /// <summary>
    /// RETRIEVES ALL LISTENING OLD SESSIONS FOR A USER.
    /// </summary>
    Task<ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>> GetListeningOldSessionsWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// SAVES A NEW LISTENING OLD SESSION.
    /// </summary>
    Task<ServiceResult<ListeningOldSessionDto>> SaveListeningOldSessionAsync(SaveListeningOldSessionRequest request);
}
