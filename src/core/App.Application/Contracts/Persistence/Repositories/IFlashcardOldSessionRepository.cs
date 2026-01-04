using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD OLD SESSION ENTITY.
/// </summary>
public interface IFlashcardOldSessionRepository
{
    /// <summary>
    /// GETS ALL FLASHCARD OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<FlashcardOldSession> items, int totalCount)> GetFlashcardOldSessionsWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(FlashcardOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<FlashcardOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE FLASHCARD OLD SESSION IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    FlashcardOldSession Update(FlashcardOldSession entity);

    /// <summary>
    /// REMOVES THE FLASHCARD OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(FlashcardOldSession entity);
}
