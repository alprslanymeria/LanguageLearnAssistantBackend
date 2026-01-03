using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardCategories.Queries.GetAllFCategoriesWithPaging;

/// <summary>
/// HANDLER FOR GET ALL FLASHCARD CATEGORIES WITH PAGING QUERY.
/// </summary>
public class GetAllFCategoriesWithPagingQueryHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IMapper mapper,
    ILogger<GetAllFCategoriesWithPagingQueryHandler> logger
    ) : IQueryHandler<GetAllFCategoriesWithPagingQuery, ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>> Handle(
        GetAllFCategoriesWithPagingQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "GetAllFCategoriesWithPagingQueryHandler -> FETCHING FLASHCARD CATEGORIES FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", 
            request.UserId, request.Page, request.PageSize);

        var (items, totalCount) = await flashcardCategoryRepository.GetAllFCategoriesWithPagingAsync(
            request.UserId, request.Page, request.PageSize);

        logger.LogInformation(
            "GetAllFCategoriesWithPagingQueryHandler -> FETCHED {Count} FLASHCARD CATEGORIES OUT OF {Total} FOR USER: {UserId}", 
            items.Count, totalCount, request.UserId);

        var mappedDtos = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(items);
        var mappedResult = new FlashcardCategoryWithTotalCount
        {
            FlashcardCategoryDtos = mappedDtos,
            TotalCount = totalCount
        };

        var pagedRequest = new PagedRequest { Page = request.Page, PageSize = request.PageSize };
        var result = PagedResult<FlashcardCategoryWithTotalCount>.Create([mappedResult], pagedRequest, totalCount);

        return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Success(result);
    }
}
