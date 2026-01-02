using App.Application.Common;
using App.Application.Features.FlashcardOldSessions.Dtos;

namespace App.Application.Features.FlashcardOldSessions;

/// <summary>
/// SERVICE INTERFACE FOR FLASHCARD OLD SESSION OPERATIONS.
/// </summary>
public interface IFlashcardOldSessionService
{
    /// <summary>
    /// RETRIEVES ALL FLASHCARD OLD SESSIONS FOR A USER.
    /// </summary>
    Task<ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>> GetFlashcardOldSessionsWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// SAVES A NEW FLASHCARD OLD SESSION.
    /// </summary>
    Task<ServiceResult<FlashcardOldSessionDto>> SaveFlashcardOldSessionAsync(SaveFlashcardOldSessionRequest request);
}
