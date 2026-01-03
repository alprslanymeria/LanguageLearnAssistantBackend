using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingBooks.Queries.GetAllWBooksWithPaging;

/// <summary>
/// HANDLER FOR GET ALL WRITING BOOKS WITH PAGING QUERY.
/// </summary>
public class GetAllWBooksWithPagingQueryHandler(
    IWritingBookRepository writingBookRepository,
    IMapper mapper,
    ILogger<GetAllWBooksWithPagingQueryHandler> logger
    ) : IQueryHandler<GetAllWBooksWithPagingQuery, ServiceResult<PagedResult<WritingBookWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<WritingBookWithTotalCount>>> Handle(
        GetAllWBooksWithPagingQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "GetAllWBooksWithPagingQueryHandler -> FETCHING WRITING BOOKS FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", 
            request.UserId, request.Page, request.PageSize);

        var (items, totalCount) = await writingBookRepository.GetAllWBooksWithPagingAsync(
            request.UserId, request.Page, request.PageSize);

        logger.LogInformation(
            "GetAllWBooksWithPagingQueryHandler -> FETCHED {Count} WRITING BOOKS OUT OF {Total} FOR USER: {UserId}", 
            items.Count, totalCount, request.UserId);

        var mappedDtos = mapper.Map<List<WritingBook>, List<WritingBookDto>>(items);
        var mappedResult = new WritingBookWithTotalCount
        {
            WritingBookDtos = mappedDtos,
            TotalCount = totalCount
        };

        var pagedRequest = new PagedRequest { Page = request.Page, PageSize = request.PageSize };
        var result = PagedResult<WritingBookWithTotalCount>.Create([mappedResult], pagedRequest, totalCount);

        return ServiceResult<PagedResult<WritingBookWithTotalCount>>.Success(result);
    }
}
