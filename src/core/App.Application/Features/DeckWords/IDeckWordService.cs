using App.Application.Common;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords;

/// <summary>
/// SERVICE INTERFACE FOR DECK WORD OPERATIONS.
/// </summary>
public interface IDeckWordService
{
    /// <summary>
    /// RETRIEVES A DECK WORD BY ID.
    /// </summary>
    Task<ServiceResult<DeckWordWithLanguageId>> GetDeckWordItemByIdAsync(int id);

    /// <summary>
    /// RETRIEVES ALL DECK WORDS WITH PAGING FOR A SPECIFIC CATEGORY.
    /// </summary>
    Task<ServiceResult<PagedResult<DeckWordWithTotalCount>>> GetAllDWordsWithPagingAsync(int categoryId, PagedRequest request);

    /// <summary>
    /// DELETES A DECK WORD BY ID.
    /// </summary>
    Task<ServiceResult> DeleteDWordItemByIdAsync(int id);

    /// <summary>
    /// CREATES A NEW DECK WORD.
    /// </summary>
    Task<ServiceResult<DeckWordDto>> DeckWordAddAsync(CreateDeckWordRequest request);

    /// <summary>
    /// UPDATES AN EXISTING DECK WORD.
    /// </summary>
    Task<ServiceResult<DeckWordDto>> DeckWordUpdateAsync(UpdateDeckWordRequest request);
}
