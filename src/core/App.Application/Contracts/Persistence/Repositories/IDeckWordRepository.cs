using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IDeckWordRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(DeckWord entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<DeckWord?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE DECK WORD IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(DeckWord entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE DECK WORD FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS A DECK WORD BY ID WITH CATEGORY INCLUDED.
    /// </summary>
    Task<DeckWordWithLanguageId?> GetDeckWordItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED DECK WORDS FOR A CATEGORY.
    /// </summary>
    Task<(List<DeckWord> Items, int TotalCount)> GetAllDWordsWithPagingAsync(string userId, int page, int pageSize);
}
