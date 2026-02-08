using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;

namespace App.Application.Features.ReadingBooks.Queries.GetAllRBooksWithPaging;

/// <summary>
/// HANDLER FOR GET ALL READING BOOKS WITH PAGING QUERY.
/// </summary>
public class GetAllRBooksWithPagingQueryHandler(

    IReadingBookRepository readingBookRepository,
    IMapper mapper

    ) : IQueryHandler<GetAllRBooksWithPagingQuery, ServiceResult<PagedResult<ReadingBookWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<ReadingBookWithTotalCount>>> Handle(

        GetAllRBooksWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await readingBookRepository.GetAllRBooksWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        var mappedDtos = mapper.Map<List<ReadingBook>, List<ReadingBookDto>>(items);

        var mappedResult = new ReadingBookWithTotalCount(

            ReadingBookDtos: mappedDtos,
            TotalCount: totalCount
            );

        var result = PagedResult<ReadingBookWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<ReadingBookWithTotalCount>>.Success(result);
    }
}
