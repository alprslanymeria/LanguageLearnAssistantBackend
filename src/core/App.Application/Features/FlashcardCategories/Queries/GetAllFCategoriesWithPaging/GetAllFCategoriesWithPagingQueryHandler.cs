using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;

namespace App.Application.Features.FlashcardCategories.Queries.GetAllFCategoriesWithPaging;

/// <summary>
/// HANDLER FOR GET ALL FLASHCARD CATEGORIES WITH PAGING QUERY.
/// </summary>
public class GetAllFCategoriesWithPagingQueryHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IMapper mapper

    ) : IQueryHandler<GetAllFCategoriesWithPagingQuery, ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>> Handle(

        GetAllFCategoriesWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await flashcardCategoryRepository.GetAllFCategoriesWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        var mappedDtos = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(items);

        var mappedResult = new FlashcardCategoryWithTotalCount(

            FlashcardCategoryDtos: mappedDtos,
            TotalCount: totalCount
            );

        var result = PagedResult<FlashcardCategoryWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Success(result);
    }
}
