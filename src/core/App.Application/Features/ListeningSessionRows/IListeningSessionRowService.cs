using App.Application.Common;
using App.Application.Features.ListeningSessionRows.Dtos;

namespace App.Application.Features.ListeningSessionRows;

/// <summary>
/// SERVICE INTERFACE FOR LISTENING SESSION ROW OPERATIONS.
/// </summary>
public interface IListeningSessionRowService
{
    /// <summary>
    /// RETRIEVES ALL LISTENING ROWS BY SESSION ID.
    /// </summary>
    Task<ServiceResult<ListeningRowsResponse>> GetListeningRowsByIdWithPagingAsync(PagedRequest request, string oldSessionId);

    /// <summary>
    /// SAVES LISTENING SESSION ROWS.
    /// </summary>
    Task<ServiceResult<List<ListeningSessionRowDto>>> SaveListeningRowsAsync(SaveListeningRowsRequest request);
}
