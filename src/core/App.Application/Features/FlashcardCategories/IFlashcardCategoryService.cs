using App.Application.Common;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories;

/// <summary>
/// SERVICE INTERFACE FOR FLASHCARD CATEGORY OPERATIONS.
/// </summary>
public interface IFlashcardCategoryService
{
    /// <summary>
    /// RETRIEVES A FLASHCARD CATEGORY BY ID.
    /// </summary>
    Task<ServiceResult<FlashcardCategoryWithLanguageId>> GetFlashcardCategoryItemByIdAsync(int id);

    /// <summary>
    /// RETRIEVES ALL FLASHCARD CATEGORIES WITH PAGING.
    /// </summary>
    Task<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>> GetAllFCategoriesWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// </summary>
    Task<ServiceResult<List<FlashcardCategoryDto>>> GetFCategoryCreateItemsAsync(string userId, string language, string practice);

    /// <summary>
    /// DELETES A FLASHCARD CATEGORY BY ID.
    /// </summary>
    Task<ServiceResult> DeleteFCategoryItemByIdAsync(int id);

    /// <summary>
    /// CREATES A NEW FLASHCARD CATEGORY.
    /// </summary>
    Task<ServiceResult<FlashcardCategoryDto>> FlashcardCategoryAddAsync(CreateFlashcardCategoryRequest request);

    /// <summary>
    /// UPDATES AN EXISTING FLASHCARD CATEGORY.
    /// </summary>
    Task<ServiceResult<FlashcardCategoryDto>> FlashcardCategoryUpdateAsync(UpdateFlashcardCategoryRequest request);
}
