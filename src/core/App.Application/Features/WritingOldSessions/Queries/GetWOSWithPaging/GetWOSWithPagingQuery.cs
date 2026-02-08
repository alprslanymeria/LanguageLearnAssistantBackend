using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingOldSessions.Dtos;

namespace App.Application.Features.WritingOldSessions.Queries.GetWOSWithPaging;

public record GetWOSWithPagingQuery(string UserId, string Language, PagedRequest Request) : IQuery<ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>>;
