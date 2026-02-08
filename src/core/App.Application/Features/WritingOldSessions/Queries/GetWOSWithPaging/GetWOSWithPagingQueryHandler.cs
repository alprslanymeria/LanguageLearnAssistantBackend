using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingOldSessions.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;

namespace App.Application.Features.WritingOldSessions.Queries.GetWOSWithPaging;

public class GetWOSWithPagingQueryHandler(

    IWritingOldSessionRepository writingOldSessionRepository,
    IMapper mapper

    ) : IQueryHandler<GetWOSWithPagingQuery, ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>> Handle(

        GetWOSWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await writingOldSessionRepository.GetWritingOldSessionsWithPagingAsync(request.UserId, request.Language, request.Request.Page, request.Request.PageSize);

        var mappedDtos = mapper.Map<List<WritingOldSession>, List<WritingOldSessionDto>>(items);

        var mappedResult = new WritingOldSessionWithTotalCount(

            WritingOldSessionDtos: mappedDtos,
            TotalCount: totalCount
            );

        var result = PagedResult<WritingOldSessionWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<WritingOldSessionWithTotalCount>>.Success(result);
    }
}
