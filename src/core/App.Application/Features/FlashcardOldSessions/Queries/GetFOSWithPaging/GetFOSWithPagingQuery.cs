using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardOldSessions.Dtos;

namespace App.Application.Features.FlashcardOldSessions.Queries.GetFOSWithPaging;

public record GetFOSWithPagingQuery(string UserId, string Language, PagedRequest Request) : IQuery<ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>>;
