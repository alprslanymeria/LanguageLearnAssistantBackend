using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;

namespace App.Application.Features.WritingBooks.Queries.GetAllWBooksWithPaging;

/// <summary>
/// HANDLER FOR GET ALL WRITING BOOKS WITH PAGING QUERY.
/// </summary>
public class GetAllWBooksWithPagingQueryHandler(

    IWritingBookRepository writingBookRepository,
    IMapper mapper

    ) : IQueryHandler<GetAllWBooksWithPagingQuery, ServiceResult<PagedResult<WritingBookWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<WritingBookWithTotalCount>>> Handle(

        GetAllWBooksWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await writingBookRepository.GetAllWBooksWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        var mappedDtos = mapper.Map<List<WritingBook>, List<WritingBookDto>>(items);

        var mappedResult = new WritingBookWithTotalCount(

            WritingBookDtos: mappedDtos,
            TotalCount: totalCount
            );

        var result = PagedResult<WritingBookWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<WritingBookWithTotalCount>>.Success(result);
    }
}
