using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingOldSessions.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingOldSessions.Queries.GetROSWithPaging;

/// <summary>
/// HANDLER FOR GET READING OLD SESSIONS WITH PAGING QUERY.
/// </summary>
public class GetROSWithPagingQueryHandler(

    IReadingOldSessionRepository readingOldSessionRepository,
    IMapper mapper,
    ILogger<GetROSWithPagingQueryHandler> logger

    ) : IQueryHandler<GetROSWithPagingQuery, ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>> Handle(

        GetROSWithPagingQuery request, 
        CancellationToken cancellationToken)
    {

        logger.LogInformation("GetROSWithPagingQueryHandler -> FETCHING  READING OLD SESSIONS FOR USER: {UserId}", request.UserId);

        var (items, totalCount) = await readingOldSessionRepository.GetReadingOldSessionsWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        logger.LogInformation("GetROSWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} READING OLD SESSIONS FOR USER: {UserId}", items.Count, request.UserId);

        var mappedDtos = mapper.Map<List<ReadingOldSession>, List<ReadingOldSessionDto>>(items);
        var mappedResult = new ReadingOldSessionWithTotalCount
        {
            ReadingOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<ReadingOldSessionWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>.Success(result);
    }
}
