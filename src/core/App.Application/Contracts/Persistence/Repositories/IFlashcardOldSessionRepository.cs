using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD OLD SESSION ENTITY.
/// </summary>
public interface IFlashcardOldSessionRepository : IGenericRepository<FlashcardOldSession, string>
{
    /// <summary>
    /// GETS ALL FLASHCARD OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<FlashcardOldSession> items, int totalCount)> GetFlashcardOldSessionsWithPagingAsync(string userId, int page, int pageSize);
}
