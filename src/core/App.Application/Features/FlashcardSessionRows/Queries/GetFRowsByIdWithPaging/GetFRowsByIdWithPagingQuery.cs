using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardSessionRows.Dtos;

namespace App.Application.Features.FlashcardSessionRows.Queries.GetFRowsByIdWithPaging;

public record GetFRowsByIdWithPagingQuery(PagedRequest Request, string OldSessionId) : IQuery<ServiceResult<FlashcardRowsResponse>>;
