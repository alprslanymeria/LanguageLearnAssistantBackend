using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingSessionRows.Dtos;

namespace App.Application.Features.WritingSessionRows.Queries.GetWRowsByIdWithPaging;

public record GetWRowsByIdWithPagingQuery(PagedRequest Request, string OldSessionId) : IQuery<ServiceResult<WritingRowsResponse>>;
