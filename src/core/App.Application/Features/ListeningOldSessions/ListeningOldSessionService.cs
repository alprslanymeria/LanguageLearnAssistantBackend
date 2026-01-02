using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ListeningOldSessions.Dtos;
using App.Domain.Entities.ListeningEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.ListeningOldSessions;

/// <summary>
/// SERVICE IMPLEMENTATION FOR LISTENING OLD SESSION OPERATIONS.
/// </summary>
public class ListeningOldSessionService(

    IListeningOldSessionRepository listeningOldSessionRepository,
    IListeningRepository listeningRepository,
    IListeningCategoryRepository listeningCategoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<ListeningOldSessionService> logger
    
    ) : IListeningOldSessionService
{

    public async Task<ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>> GetListeningOldSessionsWithPagingAsync(string userId, PagedRequest request)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("ListeningOldSessionService:GetListeningOldSessionsWithPagingAsync -> USER ID IS REQUIRED FOR FETCHING LISTENING OLD SESSIONS");
            return ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>.Fail("USER IS REQUIRED");
        }

        logger.LogInformation("ListeningOldSessionService:GetListeningOldSessionsWithPagingAsync -> FETCHING  LISTENING OLD SESSIONS FOR USER: {UserId}", userId);

        var (items, totalCount) = await listeningOldSessionRepository.GetListeningOldSessionsWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("ListeningOldSessionService:GetListeningOldSessionsWithPagingAsync -> SUCCESSFULLY FETCHED {Count} LISTENING OLD SESSIONS FOR USER: {UserId}", items.Count, userId);

        var mappedDtos = mapper.Map<List<ListeningOldSession>, List<ListeningOldSessionDto>>(items);
        var mappedResult = new ListeningOldSessionWithTotalCount
        {
            ListeningOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<ListeningOldSessionWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<ListeningOldSessionDto>> SaveListeningOldSessionAsync(SaveListeningOldSessionRequest request)
    {
        logger.LogInformation("ListeningOldSessionService:SaveListeningOldSessionAsync -> SAVING LISTENING OLD SESSION WITH ID: {SessionId}", request.Id);

        var listening = await listeningRepository.GetByIdAsync(request.ListeningId);

        // FAST FAIL
        if (listening is null)
        {
            logger.LogWarning("ListeningOldSessionService:SaveListeningOldSessionAsync -> LISTENING NOT FOUND WITH ID: {ListeningId}", request.ListeningId);
            return ServiceResult<ListeningOldSessionDto>.Fail("LISTENING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var listeningCategory = await listeningCategoryRepository.GetByIdAsync(request.ListeningCategoryId);

        // FAST FAIL
        if (listeningCategory is null)
        {
            logger.LogWarning("ListeningOldSessionService:SaveListeningOldSessionAsync -> LISTENING CATEGORY NOT FOUND WITH ID: {ListeningCategoryId}", request.ListeningCategoryId);
            return ServiceResult<ListeningOldSessionDto>.Fail("LISTENING CATEGORY NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new ListeningOldSession
        {
            Id = request.Id,
            ListeningId = request.ListeningId,
            ListeningCategoryId = request.ListeningCategoryId,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow,
            Listening = listening,
            ListeningCategory = listeningCategory
        };

        await listeningOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("ListeningOldSessionService:SaveListeningOldSessionAsync -> SUCCESSFULLY SAVED LISTENING OLD SESSION WITH ID: {SessionId}", session.Id);

        var result = mapper.Map<ListeningOldSession, ListeningOldSessionDto>(session);
        return ServiceResult<ListeningOldSessionDto>.SuccessAsCreated(result, $"/api/ListeningOldSession/{session.Id}");
    }
}
