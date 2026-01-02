using App.Application.Common;
using App.Application.Features.ReadingSessionRows.Dtos;

namespace App.Application.Features.ReadingSessionRows;

/// <summary>
/// SERVICE INTERFACE FOR READING SESSION ROW OPERATIONS.
/// </summary>
public interface IReadingSessionRowService
{
    /// <summary>
    /// RETRIEVES ALL READING ROWS BY SESSION ID.
    /// </summary>
    Task<ServiceResult<ReadingRowsResponse>> GetReadingRowsByIdWithPagingAsync(PagedRequest request, string oldSessionId);

    /// <summary>
    /// SAVES READING SESSION ROWS.
    /// </summary>
    Task<ServiceResult<List<ReadingSessionRowDto>>> SaveReadingRowsAsync(SaveReadingRowsRequest request);
}
