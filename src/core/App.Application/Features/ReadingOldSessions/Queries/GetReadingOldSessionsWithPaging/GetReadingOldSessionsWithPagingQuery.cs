using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.ReadingOldSessions.Queries.GetReadingOldSessionsWithPaging;

/// <summary>
/// QUERY FOR RETRIEVING ALL READING OLD SESSIONS WITH PAGING.
/// </summary>
public record GetReadingOldSessionsWithPagingQuery(
    string UserId, 
    int Page, 
    int PageSize
    ) : IQuery<ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>>;
