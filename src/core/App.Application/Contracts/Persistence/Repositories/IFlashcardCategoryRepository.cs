using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public interface IFlashcardCategoryRepository
{
    /// <summary>
    /// GETS A FLASHCARD CATEGORY BY ID WITH ALL DETAILS INCLUDED.
    /// </summary>
    Task<FlashcardCategory?> GetFlashcardCategoryItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED FLASHCARD CATEGORIES FOR A USER WITH ALL DETAILS.
    /// </summary>
    Task<(List<FlashcardCategory> Items, int TotalCount)> GetAllFCategoriesWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// GETS FLASHCARD CATEGORIES THAT CAN BE CREATED FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<FlashcardCategory>> GetFCategoryCreateItemsAsync(string userId, int languageId, int practiceId);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(FlashcardCategory entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<FlashcardCategory?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE FLASHCARD CATEGORY IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    FlashcardCategory Update(FlashcardCategory entity);

    /// <summary>
    /// REMOVES THE FLASHCARD CATEGORY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(FlashcardCategory entity);
}
