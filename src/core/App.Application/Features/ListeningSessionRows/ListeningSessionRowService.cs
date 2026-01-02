using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ListeningSessionRows.Dtos;
using App.Domain.Entities.ListeningEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.ListeningSessionRows;

/// <summary>
/// SERVICE IMPLEMENTATION FOR LISTENING SESSION ROW OPERATIONS.
/// </summary>
public class ListeningSessionRowService(

    IListeningSessionRowRepository listeningSessionRowRepository,
    IListeningOldSessionRepository listeningOldSessionRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<ListeningSessionRowService> logger
    
    ) : IListeningSessionRowService
{

    public async Task<ServiceResult<ListeningRowsResponse>> GetListeningRowsByIdWithPagingAsync(PagedRequest request, string oldSessionId)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(oldSessionId))
        {
            logger.LogWarning("ListeningSessionRowService:GetListeningRowsByIdWithPagingAsync -> OLD SESSION ID IS REQUIRED FOR FETCHING LISTENING   ROWS");
            return ServiceResult<ListeningRowsResponse>.Fail("OLD SESSION ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        // FIND OLD SESSION
        var oldSession = await listeningOldSessionRepository.GetByIdAsync(oldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("ListeningSessionRowService:GetListeningRowsByIdWithPagingAsync -> LISTENING OLD SESSION NOT FOUND WITH ID: {SessionId}", oldSessionId);
            return ServiceResult<ListeningRowsResponse>.Fail("LISTENING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("ListeningSessionRowService:GetListeningRowsByIdWithPagingAsync -> FETCHING LISTENING ROWS FOR OLD SESSION: {SessionId}", oldSessionId);

        var rows = await listeningSessionRowRepository.GetListeningRowsByIdWithPagingAsync(oldSessionId, request.Page, request.PageSize);

        // GET LISTENING BOOK ITEM
        var listeningBookItem = oldSession.ListeningCategory;

        logger.LogInformation("ListeningSessionRowService:GetListeningRowsByIdWithPagingAsync -> SUCCESSFULLY FETCHED {Count} LISTENING ROWS FOR SESSION: {SessionId}", rows.totalCount, oldSessionId);

        var result = mapper.Map<List<ListeningSessionRow>, List<ListeningSessionRowDto>>(rows.items);
        var response = new ListeningRowsResponse
        {
            Item = listeningBookItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<ListeningRowsResponse>.Success(response);
    }

    public async Task<ServiceResult<List<ListeningSessionRowDto>>> SaveListeningRowsAsync(SaveListeningRowsRequest request)
    {
        logger.LogInformation("ListeningSessionRowService:SaveListeningRowsAsync -> SAVING {Count} LISTENING ROWS FOR SESSION: {SessionId}", request.Rows.Count, request.ListeningOldSessionId);

        var session = await listeningOldSessionRepository.GetByIdAsync(request.ListeningOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("ListeningSessionRowService:SaveListeningRowsAsync -> LISTENING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.ListeningOldSessionId);
            return ServiceResult<List<ListeningSessionRowDto>>.Fail("LISTENING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Rows.Select(r => new ListeningSessionRow
        {
            ListeningOldSessionId = request.ListeningOldSessionId,
            ListenedSentence = r.ListenedSentence,
            Answer = r.Answer,
            Similarity = r.Similarity,
            ListeningOldSession = session

        }).ToList();

        await listeningSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("ListeningSessionRowService:SaveListeningRowsAsync -> SUCCESSFULLY SAVED {Count} LISTENING ROWS FOR SESSION: {SessionId}", rows.Count, request.ListeningOldSessionId);

        var result = mapper.Map<List<ListeningSessionRow>, List<ListeningSessionRowDto>>(rows);
        return ServiceResult<List<ListeningSessionRowDto>>.Success(result, HttpStatusCode.Created);
    }
}
