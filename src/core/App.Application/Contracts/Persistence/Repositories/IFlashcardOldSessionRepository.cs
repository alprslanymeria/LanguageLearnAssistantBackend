using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IFlashcardOldSessionRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(FlashcardOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<FlashcardOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE FLASHCARD OLD SESSION IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(FlashcardOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE FLASHCARD OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(string id);

    /// <summary>
    /// GETS ALL FLASHCARD OLD SESSIONS FOR A USER WITH PAGING AND LANGUAGE FILTER.
    /// </summary>
    Task<(List<FlashcardOldSession> Items, int TotalCount)> GetFlashcardOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize);
}
