using App.Application.Common;
using App.Application.Features.FlashcardSessionRows.Dtos;

namespace App.Application.Features.FlashcardSessionRows;

/// <summary>
/// SERVICE INTERFACE FOR FLASHCARD SESSION ROW OPERATIONS.
/// </summary>
public interface IFlashcardSessionRowService
{
    /// <summary>
    /// RETRIEVES ALL FLASHCARD ROWS BY SESSION ID.
    /// </summary>
    Task<ServiceResult<FlashcardRowsResponse>> GetFlashcardRowsByIdWithPagingAsync(PagedRequest request, string oldSessionId);

    /// <summary>
    /// SAVES FLASHCARD SESSION ROWS.
    /// </summary>
    Task<ServiceResult<List<FlashcardSessionRowDto>>> SaveFlashcardRowsAsync(SaveFlashcardRowsRequest request);
}
