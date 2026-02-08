using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ListeningOldSessions.Dtos;

namespace App.Application.Features.ListeningOldSessions.Queries.GetLOSWithPaging;

public record GetLOSWithPagingQuery(string UserId, string Language, PagedRequest Request) : IQuery<ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>>;
