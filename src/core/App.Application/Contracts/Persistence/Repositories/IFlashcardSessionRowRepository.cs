using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD SESSION ROW ENTITY.
/// </summary>
public interface IFlashcardSessionRowRepository : IGenericRepository<FlashcardSessionRow, int>
{
    /// <summary>
    /// GETS ALL FLASHCARD SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<FlashcardSessionRow> items, int totalCount)> GetFlashcardRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE FLASHCARD SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<FlashcardSessionRow> rows);
}
