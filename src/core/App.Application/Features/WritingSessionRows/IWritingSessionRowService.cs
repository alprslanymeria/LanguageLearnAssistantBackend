using App.Application.Common;
using App.Application.Features.WritingSessionRows.Dtos;

namespace App.Application.Features.WritingSessionRows;

/// <summary>
/// SERVICE INTERFACE FOR WRITING SESSION ROW OPERATIONS.
/// </summary>
public interface IWritingSessionRowService
{
    /// <summary>
    /// RETRIEVES ALL WRITING ROWS BY SESSION ID.
    /// </summary>
    Task<ServiceResult<WritingRowsResponse>> GetWritingRowsByIWithPagingAsync(PagedRequest request, string sessionId);

    /// <summary>
    /// SAVES WRITING SESSION ROWS.
    /// </summary>
    Task<ServiceResult<List<WritingSessionRowDto>>> SaveWritingRowsAsync(SaveWritingRowsRequest request);
}
