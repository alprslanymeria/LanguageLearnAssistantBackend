using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.ReadingOldSessions.Queries.GetROSWithPaging;

/// <summary>
/// QUERY FOR RETRIEVING ALL READING OLD SESSIONS WITH PAGING.
/// </summary>
public record GetROSWithPagingQuery(string UserId, PagedRequest Request) : IQuery<ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>>;
