using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ListeningSessionRows.Dtos;

namespace App.Application.Features.ListeningSessionRows.Queries.GetLRowsByIdWithPaging;

public record GetLRowsByIdWithPagingQuery(PagedRequest Request, string OldSessionId) : IQuery<ServiceResult<ListeningRowsResponse>>;
