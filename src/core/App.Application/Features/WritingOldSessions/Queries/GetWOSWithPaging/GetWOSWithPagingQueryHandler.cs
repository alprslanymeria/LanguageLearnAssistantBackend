using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingOldSessions.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingOldSessions.Queries.GetWOSWithPaging;

public class GetWOSWithPagingQueryHandler(

    IWritingOldSessionRepository writingOldSessionRepository,
    IMapper mapper,
    ILogger<GetWOSWithPagingQueryHandler> logger

    ) : IQueryHandler<GetWOSWithPagingQuery, ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>> Handle(

        GetWOSWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("GetWOSWithPagingQueryHandler -> FETCHING  WRITING OLD SESSIONS FOR USER: {UserId}", request.UserId);

        var (items, totalCount) = await writingOldSessionRepository.GetWritingOldSessionsWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        logger.LogInformation("GetWOSWithPagingQueryHandler -> SUCCESSFULLY FETCHED {Count} WRITING OLD SESSIONS FOR USER: {UserId}", items.Count, request.UserId);

        var mappedDtos = mapper.Map<List<WritingOldSession>, List<WritingOldSessionDto>>(items);
        var mappedResult = new WritingOldSessionWithTotalCount
        {
            WritingOldSessionDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<WritingOldSessionWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>.Success(result);
    }
}
