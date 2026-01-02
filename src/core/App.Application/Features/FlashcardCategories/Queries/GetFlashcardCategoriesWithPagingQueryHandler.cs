using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// HANDLER FOR GETTING ALL FLASHCARD CATEGORIES WITH PAGING.
/// </summary>
public class GetFlashcardCategoriesWithPagingQueryHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IMapper mapper,
    ILogger<GetFlashcardCategoriesWithPagingQueryHandler> logger)
    : IQueryHandler<GetFlashcardCategoriesWithPagingQuery, PagedResult<FlashcardCategoryWithTotalCount>>
{
    public async Task<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>> Handle(
        GetFlashcardCategoriesWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetFlashcardCategoriesWithPagingQueryHandler: FETCHING FLASHCARD CATEGORIES FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}",
            request.UserId, request.PagedRequest.Page, request.PagedRequest.PageSize);

        var (items, totalCount) = await flashcardCategoryRepository.GetAllFCategoriesWithPagingAsync(
            request.UserId, 
            request.PagedRequest.Page, 
            request.PagedRequest.PageSize);

        logger.LogInformation("GetFlashcardCategoriesWithPagingQueryHandler: FETCHED {Count} FLASHCARD CATEGORIES OUT OF {Total} FOR USER: {UserId}",
            items.Count, totalCount, request.UserId);

        var mappedDtos = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(items);
        var mappedResult = new FlashcardCategoryWithTotalCount
        {
            FlashcardCategoryDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<FlashcardCategoryWithTotalCount>.Create([mappedResult], request.PagedRequest, totalCount);

        return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Success(result);
    }
}
