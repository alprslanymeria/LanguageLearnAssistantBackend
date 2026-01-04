using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD SESSION ROW ENTITY.
/// </summary>
public interface IFlashcardSessionRowRepository
{
    /// <summary>
    /// GETS ALL FLASHCARD SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<FlashcardSessionRow> items, int totalCount)> GetFlashcardRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE FLASHCARD SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<FlashcardSessionRow> rows);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(FlashcardSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<FlashcardSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE FLASHCARD SESSION ROW IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    FlashcardSessionRow Update(FlashcardSessionRow entity);

    /// <summary>
    /// REMOVES THE FLASHCARD SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(FlashcardSessionRow entity);
}
