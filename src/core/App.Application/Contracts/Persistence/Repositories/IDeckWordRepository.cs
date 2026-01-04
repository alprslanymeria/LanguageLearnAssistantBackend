using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR DECK WORD ENTITY.
/// </summary>
public interface IDeckWordRepository
{
    /// <summary>
    /// GETS A DECK WORD BY ID WITH CATEGORY INCLUDED.
    /// </summary>
    Task<DeckWord?> GetDeckWordItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED DECK WORDS FOR A CATEGORY.
    /// </summary>
    Task<(List<DeckWord> Items, int TotalCount)> GetAllDWordsWithPagingAsync(int categoryId, int page, int pageSize);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(DeckWord entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<DeckWord?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE DECK WORD IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    DeckWord Update(DeckWord entity);

    /// <summary>
    /// REMOVES THE DECK WORD FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(DeckWord entity);
}
