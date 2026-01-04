using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ListeningOldSessions.Dtos;
using App.Domain.Entities.ListeningEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ListeningOldSessions.Queries.GetLOSWithPaging;

public class GetLOSWithPagingQueryHandler(

    IListeningOldSessionRepository listeningOldSessionRepository,
    IMapper mapper,
    ILogger<GetLOSWithPagingQueryHandler> logger

    ) : IQueryHandler<GetLOSWithPagingQuery, ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>>
{

    public async Task<ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>> Handle(

        GetLOSWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("GetLOSWithPagingQueryHandler -> FETCHING  LISTENING OLD SESSIONS FOR USER: {UserId}", request.UserId);

        var (items, totalCount) = await listeningOldSessionRepository.GetListeningOldSessionsWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        logger.LogInformation("GetLOSWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} LISTENING OLD SESSIONS FOR USER: {UserId}", items.Count, request.UserId);

        var mappedDtos = mapper.Map<List<ListeningOldSession>, List<ListeningOldSessionDto>>(items);
        var mappedResult = new ListeningOldSessionWithTotalCount
        {
            ListeningOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<ListeningOldSessionWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<ListeningOldSessionWithTotalCount>>.Success(result);

    }
}
