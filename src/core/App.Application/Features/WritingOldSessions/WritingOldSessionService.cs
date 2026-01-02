using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingOldSessions.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.WritingOldSessions;

/// <summary>
/// SERVICE IMPLEMENTATION FOR WRITING OLD SESSION OPERATIONS.
/// </summary>
public class WritingOldSessionService(

    IWritingOldSessionRepository writingOldSessionRepository,
    IWritingRepository writingRepository,
    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<WritingOldSessionService> logger) : IWritingOldSessionService
{
    public async Task<ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>> GetWritingOldSessionsWithPagingAsync(string userId, PagedRequest request)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("WritingOldSessionService:GetWritingOldSessionsWithPagingAsync -> USER ID IS REQUIRED FOR FETCHING WRITING OLD SESSIONS");
            return ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>.Fail("USER IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("WritingOldSessionService:GetWritingOldSessionsWithPagingAsync -> FETCHING  WRITING OLD SESSIONS FOR USER: {UserId}", userId);

        var (items, totalCount) = await writingOldSessionRepository.GetWritingOldSessionsWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("WritingOldSessionService:GetWritingOldSessionsWithPagingAsync -> SUCCESSFULLY FETCHED {Count} WRITING OLD SESSIONS FOR USER: {UserId}", items.Count, userId);

        var mappedDtos = mapper.Map<List<WritingOldSession>, List<WritingOldSessionDto>>(items);
        var mappedResult = new WritingOldSessionWithTotalCount
        {
            WritingOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<WritingOldSessionWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<WritingOldSessionDto>> SaveWritingOldSessionAsync(SaveWritingOldSessionRequest request)
    {
        logger.LogInformation("WritingOldSessionService:SaveWritingOldSessionAsync -> SAVING WRITING OLD SESSION WITH ID: {SessionId}", request.Id);

        var writing = await writingRepository.GetByIdAsync(request.WritingId);

        // FAST FAIL
        if (writing is null)
        {
            logger.LogWarning("WritingOldSessionService:SaveWritingOldSessionAsync -> WRITING NOT FOUND WITH ID: {WritingId}", request.WritingId);
            return ServiceResult<WritingOldSessionDto>.Fail("WRITING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var writingBook = await writingBookRepository.GetByIdAsync(request.WritingBookId);

        // FAST FAIL
        if (writingBook is null)
        {
            logger.LogWarning("WritingOldSessionService:SaveWritingOldSessionAsync -> WRITING BOOK NOT FOUND WITH ID: {WritingBookId}", request.WritingBookId);
            return ServiceResult<WritingOldSessionDto>.Fail("WRITING BOOK NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new WritingOldSession
        {
            Id = request.Id,
            WritingId = request.WritingId,
            WritingBookId = request.WritingBookId,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow,
            Writing = writing,
            WritingBook = writingBook
        };

        await writingOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("WritingOldSessionService:SaveWritingOldSessionAsync -> SUCCESSFULLY SAVED WRITING OLD SESSION WITH ID: {SessionId}", session.Id);

        var result = mapper.Map<WritingOldSession, WritingOldSessionDto>(session);
        return ServiceResult<WritingOldSessionDto>.SuccessAsCreated(result, $"/api/WritingOldSession/{session.Id}");
    }
}
