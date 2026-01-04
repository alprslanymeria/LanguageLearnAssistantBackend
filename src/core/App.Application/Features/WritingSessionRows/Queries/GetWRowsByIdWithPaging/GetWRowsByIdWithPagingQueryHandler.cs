using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingSessionRows.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingSessionRows.Queries.GetWRowsByIdWithPaging;

public class GetWRowsByIdWithPagingQueryHandler(

    IWritingSessionRowRepository writingSessionRowRepository,
    IWritingOldSessionRepository writingOldSessionRepository,
    IMapper mapper,
    ILogger<GetWRowsByIdWithPagingQueryHandler> logger

    ) : IQueryHandler<GetWRowsByIdWithPagingQuery, ServiceResult<WritingRowsResponse>>
{
    public async Task<ServiceResult<WritingRowsResponse>> Handle(

        GetWRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        // FIND OLD SESSION
        var oldSession = await writingOldSessionRepository.GetByIdAsync(request.OldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("GetWRowsByIdWithPagingQueryHandler -> WRITING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.OldSessionId);
            return ServiceResult<WritingRowsResponse>.Fail("WRITING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("GetWRowsByIdWithPagingQueryHandler -> FETCHING WRITING ROWS FOR OLD SESSION: {SessionId}", request.OldSessionId);

        var rows = await writingSessionRowRepository.GetWritingRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET WRITING BOOK ITEM
        var writingBookItem = oldSession.WritingBook;

        logger.LogInformation("GetWRowsByIdWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} WRITING ROWS FOR SESSION: {SessionId}", rows.totalCount, request.OldSessionId);

        var result = mapper.Map<List<WritingSessionRow>, List<WritingSessionRowDto>>(rows.items);
        var response = new WritingRowsResponse
        {
            Item = writingBookItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<WritingRowsResponse>.Success(response);
    }
}
