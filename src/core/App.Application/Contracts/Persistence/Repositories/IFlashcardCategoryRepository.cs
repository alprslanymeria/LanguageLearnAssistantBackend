using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public interface IFlashcardCategoryRepository : IGenericRepository<FlashcardCategory, int>
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
}
