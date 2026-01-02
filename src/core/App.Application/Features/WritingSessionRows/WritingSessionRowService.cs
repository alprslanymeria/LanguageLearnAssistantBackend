using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingSessionRows.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.WritingSessionRows;

/// <summary>
/// SERVICE IMPLEMENTATION FOR WRITING SESSION ROW OPERATIONS.
/// </summary>
public class WritingSessionRowService(

    IWritingSessionRowRepository writingSessionRowRepository,
    IWritingOldSessionRepository writingOldSessionRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<WritingSessionRowService> logger
    
    ) : IWritingSessionRowService
{

    public async Task<ServiceResult<WritingRowsResponse>> GetWritingRowsByIWithPagingAsync(PagedRequest request , string oldSessionId)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(oldSessionId))
        {
            logger.LogWarning("WritingSessionRowService:GetWritingRowsByIWithPagingAsync -> OLD SESSION ID IS REQUIRED FOR FETCHING WRITING ROWS");
            return ServiceResult<WritingRowsResponse>.Fail("OLD SESSION ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        // FIND OLD SESSION
        var oldSession = await writingOldSessionRepository.GetByIdAsync(oldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("WritingSessionRowService:GetWritingRowsByIWithPagingAsync -> WRITING OLD SESSION NOT FOUND WITH ID: {SessionId}", oldSessionId);
            return ServiceResult<WritingRowsResponse>.Fail("WRITING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("WritingSessionRowService:GetWritingRowsByIWithPagingAsync -> FETCHING WRITING ROWS FOR OLD SESSION: {SessionId}", oldSessionId);

        var rows = await writingSessionRowRepository.GetWritingRowsByIdWithPagingAsync(oldSessionId, request.Page, request.PageSize);

        // GET WRITING BOOK ITEM
        var writingBookItem = oldSession.WritingBook;

        logger.LogInformation("WritingSessionRowService:GetWritingRowsByIdAsync -> SUCCESSFULLY FETCHED {Count} WRITING ROWS FOR SESSION: {SessionId}", rows.totalCount, oldSessionId);

        var result = mapper.Map<List<WritingSessionRow>, List<WritingSessionRowDto>>(rows.items);
        var response = new WritingRowsResponse
        {
            Item = writingBookItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<WritingRowsResponse>.Success(response);
    }

    public async Task<ServiceResult<List<WritingSessionRowDto>>> SaveWritingRowsAsync(SaveWritingRowsRequest request)
    {
        logger.LogInformation("WritingSessionRowService:SaveWritingRowsAsync -> SAVING {Count} WRITING ROWS FOR SESSION: {SessionId}", request.Rows.Count, request.WritingOldSessionId);

        var session = await writingOldSessionRepository.GetByIdAsync(request.WritingOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("WritingSessionRowService:SaveWritingRowsAsync -> WRITING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.WritingOldSessionId);
            return ServiceResult<List<WritingSessionRowDto>>.Fail("WRITING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Rows.Select(r => new WritingSessionRow
        {
            WritingOldSessionId = request.WritingOldSessionId,
            SelectedSentence = r.SelectedSentence,
            Answer = r.Answer,
            AnswerTranslate = r.AnswerTranslate,
            Similarity = r.Similarity,
            WritingOldSession = session

        }).ToList();

        await writingSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("WritingSessionRowService:SaveWritingRowsAsync -> SUCCESSFULLY SAVED {Count} WRITING ROWS FOR SESSION: {SessionId}", rows.Count, request.WritingOldSessionId);

        var result = mapper.Map<List<WritingSessionRow>, List<WritingSessionRowDto>>(rows);
        return ServiceResult<List<WritingSessionRowDto>>.Success(result, HttpStatusCode.Created);
    }
}
