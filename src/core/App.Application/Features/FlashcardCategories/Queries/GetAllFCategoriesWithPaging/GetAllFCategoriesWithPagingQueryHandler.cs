using System.Net;
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
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            logger.LogWarning("GetAllFCategoriesWithPagingQueryHandler --> USER ID IS REQUIRED FOR FETCHING FLASHCARD CATEGORIES");
            return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("GetAllFCategoriesWithPagingQueryHandler --> FETCHING FLASHCARD CATEGORIES FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", request.UserId, request.Request.Page, request.Request.PageSize);

        var (items, totalCount) = await flashcardCategoryRepository.GetAllFCategoriesWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        logger.LogInformation("GetAllFCategoriesWithPagingQueryHandler --> FETCHED {Count} FLASHCARD CATEGORIES OUT OF {Total} FOR USER: {UserId}", items.Count, totalCount, request.UserId);

        var mappedDtos = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(items);
        var mappedResult = new FlashcardCategoryWithTotalCount
        {
            FlashcardCategoryDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<FlashcardCategoryWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Success(result);
    }
}
