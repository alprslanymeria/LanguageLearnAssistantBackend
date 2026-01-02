using System.Net;
using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingSessionRows.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingSessionRows;

/// <summary>
/// SERVICE IMPLEMENTATION FOR READING SESSION ROW OPERATIONS.
/// </summary>
public class ReadingSessionRowService(

    IReadingSessionRowRepository readingSessionRowRepository,
    IReadingOldSessionRepository readingOldSessionRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<ReadingSessionRowService> logger
    
    ) : IReadingSessionRowService
{
    public async Task<ServiceResult<ReadingRowsResponse>> GetReadingRowsByIdWithPagingAsync(PagedRequest request, string oldSessionId)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(oldSessionId))
        {
            logger.LogWarning("ReadingSessionRowService:GetReadingRowsByIdWithPagingAsync -> OLD SESSION ID IS REQUIRED FOR FETCHING READING ROWS");
            return ServiceResult<ReadingRowsResponse>.Fail("OLD SESSION ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        // FIND OLD SESSION
        var oldSession = await readingOldSessionRepository.GetByIdAsync(oldSessionId);

        if(oldSession is null)
        {
            logger.LogWarning("ReadingSessionRowService:GetReadingRowsByIdWithPagingAsync -> READING OLD SESSION NOT FOUND WITH ID: {SessionId}", oldSessionId);
            return ServiceResult<ReadingRowsResponse>.Fail("READING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("ReadingSessionRowService:GetReadingRowsByIdWithPagingAsync -> FETCHING READING ROWS FOR OLD SESSION: {SessionId}", oldSessionId);

        var rows = await readingSessionRowRepository.GetReadingRowsByIdWithPagingAsync(oldSessionId, request.Page, request.PageSize);

        // GET READING BOOK ITEM
        var readingBookItem = oldSession.ReadingBook;

        logger.LogInformation("ReadingSessionRowService:GetReadingRowsByIdWithPagingAsync -> SUCCESSFULLY FETCHED {Count} READING ROWS FOR SESSION: {SessionId}", rows.totalCount, oldSessionId);

        var result = mapper.Map<List<ReadingSessionRow>, List<ReadingSessionRowDto>>(rows.items);
        var response = new ReadingRowsResponse
        {
            Item = readingBookItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<ReadingRowsResponse>.Success(response);
    }

    public async Task<ServiceResult<List<ReadingSessionRowDto>>> SaveReadingRowsAsync(SaveReadingRowsRequest request)
    {

        logger.LogInformation("ReadingSessionRowService:SaveReadingRowsAsync -> SAVING {Count} READING ROWS FOR SESSION: {SessionId}", request.Rows.Count, request.ReadingOldSessionId);

        var session = await readingOldSessionRepository.GetByIdAsync(request.ReadingOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("ReadingSessionRowService:SaveReadingRowsAsync -> READING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.ReadingOldSessionId);
            return ServiceResult<List<ReadingSessionRowDto>>.Fail("READING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Rows.Select(r => new ReadingSessionRow
        {
            ReadingOldSessionId = request.ReadingOldSessionId,
            SelectedSentence = r.SelectedSentence,
            Answer = r.Answer,
            AnswerTranslate = r.AnswerTranslate,
            Similarity = r.Similarity,
            ReadingOldSession = session

        }).ToList();

        await readingSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("ReadingSessionRowService:SaveReadingRowsAsync -> SUCCESSFULLY SAVED {Count} READING ROWS FOR SESSION: {SessionId}", rows.Count, request.ReadingOldSessionId);

        var result = mapper.Map<List<ReadingSessionRow>, List<ReadingSessionRowDto>>(rows);
        return ServiceResult<List<ReadingSessionRowDto>>.Success(result, HttpStatusCode.Created);
    }
}
