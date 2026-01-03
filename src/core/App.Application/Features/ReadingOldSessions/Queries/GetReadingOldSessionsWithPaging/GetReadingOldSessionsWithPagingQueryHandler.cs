using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingOldSessions.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingOldSessions.Queries.GetReadingOldSessionsWithPaging;

/// <summary>
/// HANDLER FOR GET READING OLD SESSIONS WITH PAGING QUERY.
/// </summary>
public class GetReadingOldSessionsWithPagingQueryHandler(
    IReadingOldSessionRepository readingOldSessionRepository,
    IMapper mapper,
    ILogger<GetReadingOldSessionsWithPagingQueryHandler> logger
    ) : IQueryHandler<GetReadingOldSessionsWithPagingQuery, ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>> Handle(
        GetReadingOldSessionsWithPagingQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "GetReadingOldSessionsWithPagingQueryHandler -> FETCHING READING OLD SESSIONS FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", 
            request.UserId, request.Page, request.PageSize);

        var (items, totalCount) = await readingOldSessionRepository.GetReadingOldSessionsWithPagingAsync(
            request.UserId, request.Page, request.PageSize);

        logger.LogInformation(
            "GetReadingOldSessionsWithPagingQueryHandler -> FETCHED {Count} READING OLD SESSIONS OUT OF {Total} FOR USER: {UserId}", 
            items.Count, totalCount, request.UserId);

        var mappedDtos = mapper.Map<List<ReadingOldSession>, List<ReadingOldSessionDto>>(items);
        var mappedResult = new ReadingOldSessionWithTotalCount
        {
            ReadingOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var pagedRequest = new PagedRequest { Page = request.Page, PageSize = request.PageSize };
        var result = PagedResult<ReadingOldSessionWithTotalCount>.Create([mappedResult], pagedRequest, totalCount);

        return ServiceResult<PagedResult<ReadingOldSessionWithTotalCount>>.Success(result);
    }
}
