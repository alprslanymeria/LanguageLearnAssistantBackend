using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.DeckWords.Queries.GetAllDWordsWithPaging;

/// <summary>
/// HANDLER FOR GET ALL DECK WORDS WITH PAGING QUERY.
/// </summary>
public class GetAllDWordsWithPagingQueryHandler(
    IDeckWordRepository deckWordRepository,
    IMapper mapper,
    ILogger<GetAllDWordsWithPagingQueryHandler> logger
    ) : IQueryHandler<GetAllDWordsWithPagingQuery, ServiceResult<PagedResult<DeckWordWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<DeckWordWithTotalCount>>> Handle(
        GetAllDWordsWithPagingQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "GetAllDWordsWithPagingQueryHandler -> FETCHING DECK WORDS FOR CATEGORY: {CategoryId}, PAGE: {Page}, PAGE SIZE: {PageSize}", 
            request.CategoryId, request.Page, request.PageSize);

        var (items, totalCount) = await deckWordRepository.GetAllDWordsWithPagingAsync(
            request.CategoryId, request.Page, request.PageSize);

        logger.LogInformation(
            "GetAllDWordsWithPagingQueryHandler -> FETCHED {Count} DECK WORDS OUT OF {Total} FOR CATEGORY: {CategoryId}", 
            items.Count, totalCount, request.CategoryId);

        var mappedDtos = mapper.Map<List<DeckWord>, List<DeckWordDto>>(items);
        var mappedResult = new DeckWordWithTotalCount
        {
            DeckWordDtos = mappedDtos,
            TotalCount = totalCount
        };

        var pagedRequest = new PagedRequest { Page = request.Page, PageSize = request.PageSize };
        var result = PagedResult<DeckWordWithTotalCount>.Create([mappedResult], pagedRequest, totalCount);

        return ServiceResult<PagedResult<DeckWordWithTotalCount>>.Success(result);
    }
}
