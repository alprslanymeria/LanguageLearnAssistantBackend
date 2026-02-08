using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardOldSessions.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;

namespace App.Application.Features.FlashcardOldSessions.Queries.GetFOSWithPaging;

/// <summary>
/// HANDLER FOR GETTING FLASHCARD OLD SESSIONS WITH PAGING.
/// </summary>
public class GetFOSWithPagingQueryHandler(

    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IMapper mapper

    ) : IQueryHandler<GetFOSWithPagingQuery, ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>> Handle(

        GetFOSWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await flashcardOldSessionRepository.GetFlashcardOldSessionsWithPagingAsync(request.UserId, request.Language, request.Request.Page, request.Request.PageSize);

        var mappedDtos = mapper.Map<List<FlashcardOldSession>, List<FlashcardOldSessionDto>>(items);

        var mappedResult = new FlashcardOldSessionWithTotalCount(

            FlashcardOldSessionDtos: mappedDtos,
            TotalCount: totalCount
            );

        var result = PagedResult<FlashcardOldSessionWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>.Success(result);
    }
}
