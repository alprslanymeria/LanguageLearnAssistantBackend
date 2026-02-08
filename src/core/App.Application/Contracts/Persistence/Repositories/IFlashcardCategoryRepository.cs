using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IFlashcardCategoryRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(FlashcardCategory entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<FlashcardCategory?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE FLASHCARD CATEGORY IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(FlashcardCategory entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE FLASHCARD CATEGORY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS A FLASHCARD CATEGORY BY ID WITH LANGUAGE DETAILS INCLUDED.
    /// </summary>
    Task<FlashcardCategoryWithLanguageId?> GetFlashcardCategoryItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED FLASHCARD CATEGORIES FOR A USER.
    /// </summary>
    Task<(List<FlashcardCategory> Items, int TotalCount)> GetAllFCategoriesWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// GETS ALL FLASHCARD CATEGORIES FOR A USER WITH LANGUAGE DETAILS.
    /// </summary>
    Task<(List<FlashcardCategoryWithLanguageId> Items, int TotalCount)> GetAllFCategoriesAsync(string userId);

    /// <summary>
    /// GETS FLASHCARD CATEGORIES WITH DECK WORDS FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<FlashcardCategoryWithDeckWords>> GetFCategoryCreateItemsAsync(string userId, int languageId, int practiceId);

    /// <summary>
    /// GETS A FLASHCARD CATEGORY BY ID WITH DECK WORDS INCLUDED.
    /// </summary>
    Task<FlashcardCategoryWithDeckWords?> GetByIdWithDeckWordsAsync(int id);
}
