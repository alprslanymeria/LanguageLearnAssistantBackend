using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardSessionRows.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardSessionRows;

/// <summary>
/// SERVICE IMPLEMENTATION FOR FLASHCARD SESSION ROW OPERATIONS.
/// </summary>
public class FlashcardSessionRowService(

    IFlashcardSessionRowRepository flashcardSessionRowRepository,
    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<FlashcardSessionRowService> logger
    
    ) : IFlashcardSessionRowService
{

    public async Task<ServiceResult<FlashcardRowsResponse>> GetFlashcardRowsByIdWithPagingAsync(PagedRequest request, string oldSessionId)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(oldSessionId))
        {
            logger.LogWarning("FlashcardSessionRowService:GetFlashcardRowsByIdWithPagingAsync -> OLD SESSION ID IS REQUIRED FOR FETCHING FLASHCARD ROWS");
            return ServiceResult<FlashcardRowsResponse>.Fail("OLD SESSION ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        // FIND OLD SESSION
        var oldSession = await flashcardOldSessionRepository.GetByIdAsync(oldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("FlashcardSessionRowService:GetFlashcardRowsByIdWithPagingAsync -> FLASHCARD OLD SESSION NOT FOUND WITH ID: {SessionId}", oldSessionId);
            return ServiceResult<FlashcardRowsResponse>.Fail("FLASHCARD OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("FlashcardSessionRowService:GetFlashcardRowsByIdWithPagingAsync -> FETCHING FLASHCARD ROWS FOR OLD SESSION: {SessionId}", oldSessionId);

        var rows = await flashcardSessionRowRepository.GetFlashcardRowsByIdWithPagingAsync(oldSessionId, request.Page, request.PageSize);

        // GET FLASHCARD CATEGORY ITEM
        var flashcardCategoryItem = oldSession.FlashcardCategory;

        logger.LogInformation("FlashcardSessionRowService:GetFlashcardRowsByIdWithPagingAsync -> SUCCESSFULLY FETCHED {Count} FLASHCARD ROWS FOR SESSION: {SessionId}", rows.totalCount, oldSessionId);

        var result = mapper.Map<List<FlashcardSessionRow>, List<FlashcardSessionRowDto>>(rows.items);
        var response = new FlashcardRowsResponse
        {
            Item = flashcardCategoryItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<FlashcardRowsResponse>.Success(response);
    }

    public async Task<ServiceResult<List<FlashcardSessionRowDto>>> SaveFlashcardRowsAsync(SaveFlashcardRowsRequest request)
    {
        logger.LogInformation("FlashcardSessionRowService:SaveFlashcardRowsAsync -> SAVING {Count} FLASHCARD ROWS FOR SESSION: {SessionId}", request.Rows.Count, request.FlashcardOldSessionId);

        var session = await flashcardOldSessionRepository.GetByIdAsync(request.FlashcardOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("FlashcardSessionRowService:SaveFlashcardRowsAsync -> FLASHCARD OLD SESSION NOT FOUND WITH ID: {SessionId}", request.FlashcardOldSessionId);
            return ServiceResult<List<FlashcardSessionRowDto>>.Fail("FLASHCARD OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Rows.Select(r => new FlashcardSessionRow
        {
            FlashcardOldSessionId = request.FlashcardOldSessionId,
            Answer = r.Answer,
            Question = r.Question,
            Status = r.Status,
            FlashcardOldSession = session

        }).ToList();

        await flashcardSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("FlashcardSessionRowService:SaveFlashcardRowsAsync -> SUCCESSFULLY SAVED {Count} FLASHCARD ROWS FOR SESSION: {SessionId}", rows.Count, request.FlashcardOldSessionId);

        var result = mapper.Map<List<FlashcardSessionRow>, List<FlashcardSessionRowDto>>(rows);
        return ServiceResult<List<FlashcardSessionRowDto>>.Success(result, HttpStatusCode.Created);
    }
}
