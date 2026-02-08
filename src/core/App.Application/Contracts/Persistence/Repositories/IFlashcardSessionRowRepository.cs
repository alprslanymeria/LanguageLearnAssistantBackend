using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IFlashcardSessionRowRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(FlashcardSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<FlashcardSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE FLASHCARD SESSION ROW IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(FlashcardSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE FLASHCARD SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS ALL FLASHCARD SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<FlashcardSessionRow> Items, int TotalCount)> GetFlashcardRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE FLASHCARD SESSION ROWS.
    /// </summary>
    Task AddRangeAsync(IEnumerable<FlashcardSessionRow> rows);
}
