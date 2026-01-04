using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingSessionRows.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingSessionRows.Queries.GetRRowsByIdWithPaging;

public class GetRRowsByIdWithPagingQueryHandler(

    IReadingSessionRowRepository readingSessionRowRepository,
    IReadingOldSessionRepository readingOldSessionRepository,
    IMapper mapper,
    ILogger<GetRRowsByIdWithPagingQueryHandler> logger

    ) : IQueryHandler<GetRRowsByIdWithPagingQuery, ServiceResult<ReadingRowsResponse>>
{
    public async Task<ServiceResult<ReadingRowsResponse>> Handle(

        GetRRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        // FIND OLD SESSION
        var oldSession = await readingOldSessionRepository.GetByIdAsync(request.OldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("GetRRowsByIdWithPagingQueryHandler -> READING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.OldSessionId);
            return ServiceResult<ReadingRowsResponse>.Fail("READING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("GetRRowsByIdWithPagingQueryHandler -> FETCHING READING ROWS FOR OLD SESSION: {SessionId}", request.OldSessionId);

        var rows = await readingSessionRowRepository.GetReadingRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET READING BOOK ITEM
        var readingBookItem = oldSession.ReadingBook;

        logger.LogInformation("GetRRowsByIdWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} READING ROWS FOR SESSION: {SessionId}", rows.totalCount, request.OldSessionId);

        var result = mapper.Map<List<ReadingSessionRow>, List<ReadingSessionRowDto>>(rows.items);
        var response = new ReadingRowsResponse
        {
            Item = readingBookItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<ReadingRowsResponse>.Success(response);
    }
}
