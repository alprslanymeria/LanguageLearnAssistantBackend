using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingOldSessions.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.ReadingOldSessions;

/// <summary>
/// SERVICE IMPLEMENTATION FOR READING OLD SESSION OPERATIONS.
/// </summary>
public class ReadingOldSessionService(

    IReadingOldSessionRepository readingOldSessionRepository,
    IReadingRepository readingRepository,
    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<ReadingOldSessionService> logger
    
    ) : IReadingOldSessionService
{

    public async Task<ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>> GetReadingOldSessionsWithPagingAsync(string userId, PagedRequest request)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("ReadingOldSessionService:GetReadingOldSessionsAsync -> USER ID IS REQUIRED FOR FETCHING READING OLD SESSIONS");
            return ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>.Fail("USER IS REQUIRED");
        }

        logger.LogInformation("ReadingOldSessionService:GetReadingOldSessionsAsync -> FETCHING  READING OLD SESSIONS FOR USER: {UserId}", userId);

        var (items, totalCount) = await readingOldSessionRepository.GetReadingOldSessionsWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("ReadingOldSessionService:GetReadingOldSessionsAsync -> SUCCESSFULLY FETCHED {Count} READING OLD SESSIONS FOR USER: {UserId}", items.Count, userId);

        var mappedDtos = mapper.Map<List<ReadingOldSession>, List<ReadingOldSessionDto>>(items);
        var mappedResult = new ReadingOldSessionWithTotalCount
        {
            ReadingOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<ReadingOldSessionWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<ReadingOldSessionDto>> SaveReadingOldSessionAsync(SaveReadingOldSessionRequest request)
    {

        logger.LogInformation("ReadingOldSessionService:SaveReadingOldSessionAsync -> SAVING READING OLD SESSION WITH ID: {SessionId}", request.Id);

        var reading = await readingRepository.GetByIdAsync(request.ReadingId);

        // FAST FAIL
        if (reading is null)
        {
            logger.LogWarning("ReadingOldSessionService:SaveReadingOldSessionAsync -> READING NOT FOUND WITH ID: {ReadingId}", request.ReadingId);
            return ServiceResult<ReadingOldSessionDto>.Fail("READING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var readingBook = await readingBookRepository.GetByIdAsync(request.ReadingBookId);

        // FAST FAIL
        if (readingBook is null)
        {
            logger.LogWarning("ReadingOldSessionService:SaveReadingOldSessionAsync -> READING BOOK NOT FOUND WITH ID: {ReadingBookId}", request.ReadingBookId);
            return ServiceResult<ReadingOldSessionDto>.Fail("READING BOOK NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new ReadingOldSession
        {
            Id = request.Id,
            ReadingId = request.ReadingId,
            ReadingBookId = request.ReadingBookId,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow,
            Reading = reading,
            ReadingBook = readingBook
        };

        await readingOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("ReadingOldSessionService:SaveReadingOldSessionAsync -> SUCCESSFULLY SAVED READING OLD SESSION WITH ID: {SessionId}", session.Id);

        var result = mapper.Map<ReadingOldSession, ReadingOldSessionDto>(session);
        return ServiceResult<ReadingOldSessionDto>.SuccessAsCreated(result, $"/api/ReadingOldSession/{session.Id}");
    }
}
