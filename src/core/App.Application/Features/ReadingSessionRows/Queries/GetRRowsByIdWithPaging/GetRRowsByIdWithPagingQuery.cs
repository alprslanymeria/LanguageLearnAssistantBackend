using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingSessionRows.Dtos;

namespace App.Application.Features.ReadingSessionRows.Queries.GetRRowsByIdWithPaging;

public record GetRRowsByIdWithPagingQuery(PagedRequest Request, string OldSessionId) : IQuery<ServiceResult<ReadingRowsResponse>>;
