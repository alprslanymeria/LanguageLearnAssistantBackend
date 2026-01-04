using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ListeningSessionRows.Dtos;
using App.Domain.Entities.ListeningEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ListeningSessionRows.Queries.GetLRowsByIdWithPaging;

public class GetLRowsByIdWithPagingQueryHandler(

    IListeningSessionRowRepository listeningSessionRowRepository,
    IListeningOldSessionRepository listeningOldSessionRepository,
    IMapper mapper,
    ILogger<GetLRowsByIdWithPagingQueryHandler> logger

    ) : IQueryHandler<GetLRowsByIdWithPagingQuery, ServiceResult<ListeningRowsResponse>>
{
    public async Task<ServiceResult<ListeningRowsResponse>> Handle(

        GetLRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        // FIND OLD SESSION
        var oldSession = await listeningOldSessionRepository.GetByIdAsync(request.OldSessionId);

        if (oldSession is null)
        {
            logger.LogWarning("GetLRowsByIdWithPagingQueryHandler -> LISTENING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.OldSessionId);
            return ServiceResult<ListeningRowsResponse>.Fail("LISTENING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        // GET ROWS
        logger.LogInformation("GetLRowsByIdWithPagingQueryHandler -> FETCHING LISTENING ROWS FOR OLD SESSION: {SessionId}", request.OldSessionId);

        var rows = await listeningSessionRowRepository.GetListeningRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET LISTENING BOOK ITEM
        var listeningBookItem = oldSession.ListeningCategory;

        logger.LogInformation("GetLRowsByIdWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} LISTENING ROWS FOR SESSION: {SessionId}", rows.totalCount, request.OldSessionId);

        var result = mapper.Map<List<ListeningSessionRow>, List<ListeningSessionRowDto>>(rows.items);
        var response = new ListeningRowsResponse
        {
            Item = listeningBookItem,
            Contents = result,
            Total = rows.totalCount
        };

        return ServiceResult<ListeningRowsResponse>.Success(response);
    }
}
