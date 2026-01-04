using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardSessionRows.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardSessionRows.Queries.GetFRowsByIdWithPaging;

public class GetFRowsByIdWithPagingQueryHandler(

    IFlashcardSessionRowRepository flashcardSessionRowRepository,
    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IMapper mapper,
    ILogger<GetFRowsByIdWithPagingQueryHandler> logger

    ) : IQueryHandler<GetFRowsByIdWithPagingQuery, ServiceResult<FlashcardRowsResponse>>
{
    public async Task<ServiceResult<FlashcardRowsResponse>> Handle(

        GetFRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        // FIND OLD SESSION
        var oldSession = await flashcardOldSessionRepository.GetByIdAsync(request.OldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("GetFRowsByIdWithPagingHandler -> FLASHCARD OLD SESSION NOT FOUND WITH ID: {SessionId}", request.OldSessionId);
            return ServiceResult<FlashcardRowsResponse>.Fail("FLASHCARD OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("GetFRowsByIdWithPagingHandler -> FETCHING FLASHCARD ROWS FOR OLD SESSION: {SessionId}", request.OldSessionId);

        var rows = await flashcardSessionRowRepository.GetFlashcardRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET FLASHCARD CATEGORY ITEM
        var flashcardCategoryItem = oldSession.FlashcardCategory;

        logger.LogInformation("GetFRowsByIdWithPagingHandler -> SUCCESSFULLY FETCHED {Count} FLASHCARD ROWS FOR SESSION: {SessionId}", rows.totalCount, request.OldSessionId);

        var result = mapper.Map<List<FlashcardSessionRow>, List<FlashcardSessionRowDto>>(rows.items);
        var response = new FlashcardRowsResponse
        {
            Item = flashcardCategoryItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<FlashcardRowsResponse>.Success(response);
    }
}
