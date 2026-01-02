using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardOldSessions.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardOldSessions;

/// <summary>
/// SERVICE IMPLEMENTATION FOR FLASHCARD OLD SESSION OPERATIONS.
/// </summary>
public class FlashcardOldSessionService(

    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IFlashcardRepository flashcardRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<FlashcardOldSessionService> logger
    
    ) : IFlashcardOldSessionService
{

    public async Task<ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>> GetFlashcardOldSessionsWithPagingAsync(string userId, PagedRequest request)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("FlashcardOldSessionService:GetFlashcardOldSessionsAsync -> USER ID IS REQUIRED FOR FETCHING FLASHCARD OLD SESSIONS");
            return ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>.Fail("USER IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("FlashcardOldSessionService:GetFlashcardOldSessionsWithPagingAsync -> FETCHING  FLASHCARD OLD SESSIONS FOR USER: {UserId}", userId);

        var (items, totalCount) = await flashcardOldSessionRepository.GetFlashcardOldSessionsWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("FlashcardOldSessionService:GetFlashcardOldSessionsWithPagingAsync -> SUCCESSFULLY FETCHED {Count} FLASHCARD OLD SESSIONS FOR USER: {UserId}", items.Count, userId);

        var mappedDtos = mapper.Map<List<FlashcardOldSession>, List<FlashcardOldSessionDto>>(items);
        var mappedResult = new FlashcardOldSessionWithTotalCount
        {
            FlashcardOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<FlashcardOldSessionWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<FlashcardOldSessionWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<FlashcardOldSessionDto>> SaveFlashcardOldSessionAsync(SaveFlashcardOldSessionRequest request)
    {
        logger.LogInformation("FlashcardOldSessionService:SaveFlashcardOldSessionAsync -> SAVING FLASHCARD OLD SESSION WITH ID: {SessionId}", request.Id);

        var flashcard = await flashcardRepository.GetByIdAsync(request.FlashcardId);

        // FAST FAIL
        if (flashcard is null)
        {
            logger.LogWarning("FlashcardOldSessionService:SaveFlashcardOldSessionAsync -> FLASHCARD NOT FOUND WITH ID: {FlashcardId}", request.FlashcardId);
            return ServiceResult<FlashcardOldSessionDto>.Fail("FLASHCARD NOT FOUND.", HttpStatusCode.NotFound);
        }

        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.FlashcardCategoryId);

        // FAST FAIL
        if (flashcardCategory is null)
        {
            logger.LogWarning("FlashcardOldSessionService:SaveFlashcardOldSessionAsync -> FLASHCARD CATEGORY NOT FOUND WITH ID: {FlashcardCategoryId}", request.FlashcardCategoryId);
            return ServiceResult<FlashcardOldSessionDto>.Fail("FLASHCARD CATEGORY NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new FlashcardOldSession
        {
            Id = request.Id,
            FlashcardId = request.FlashcardId,
            FlashcardCategoryId = request.FlashcardCategoryId,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow,
            Flashcard = flashcard,
            FlashcardCategory = flashcardCategory
        };

        await flashcardOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("FlashcardOldSessionService:SaveFlashcardOldSessionAsync -> SUCCESSFULLY SAVED FLASHCARD OLD SESSION WITH ID: {SessionId}", session.Id);

        var result = mapper.Map<FlashcardOldSession, FlashcardOldSessionDto>(session);
        return ServiceResult<FlashcardOldSessionDto>.SuccessAsCreated(result, $"/api/FlashcardOldSession/{session.Id}");
    }
}
