using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR DECK WORD ENTITY.
/// </summary>
public interface IDeckWordRepository : IGenericRepository<DeckWord, int>
{
    /// <summary>
    /// GETS A DECK WORD BY ID WITH CATEGORY INCLUDED.
    /// </summary>
    Task<DeckWord?> GetDeckWordItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED DECK WORDS FOR A CATEGORY.
    /// </summary>
    Task<(List<DeckWord> Items, int TotalCount)> GetAllDWordsWithPagingAsync(int categoryId, int page, int pageSize);
}
