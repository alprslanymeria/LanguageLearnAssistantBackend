using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardOldSessions.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardOldSessions.Queries.GetFOSWithPaging;

/// <summary>
/// HANDLER FOR GETTING FLASHCARD OLD SESSIONS WITH PAGING.
/// </summary>
public class GetFOSWithPagingQueryHandler(

    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IMapper mapper,
    ILogger<GetFOSWithPagingQueryHandler> logger

    ) : IQueryHandler<GetFOSWithPagingQuery, ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>> Handle(

        GetFOSWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("GetFOSWithPagingQueryHandler -> FETCHING  FLASHCARD OLD SESSIONS FOR USER: {UserId}", request.UserId);

        var (items, totalCount) = await flashcardOldSessionRepository.GetFlashcardOldSessionsWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        logger.LogInformation("GetFOSWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} FLASHCARD OLD SESSIONS FOR USER: {UserId}", items.Count, request.UserId);

        var mappedDtos = mapper.Map<List<FlashcardOldSession>, List<FlashcardOldSessionDto>>(items);
        var mappedResult = new FlashcardOldSessionWithTotalCount
        {
            FlashcardOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<FlashcardOldSessionWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>.Success(result);
    }
}
