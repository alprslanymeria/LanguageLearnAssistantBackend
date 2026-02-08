using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries.GetAllFCategories;

public class GetAllFCategoriesQueryHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository

    ) : IQueryHandler<GetAllFCategoriesQuery, ServiceResult<FlashcardCategoryWithLanguageIds>>
{
    public async Task<ServiceResult<FlashcardCategoryWithLanguageIds>> Handle(

        GetAllFCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await flashcardCategoryRepository.GetAllFCategoriesAsync(request.UserId);

        var result = new FlashcardCategoryWithLanguageIds(items, totalCount);

        return ServiceResult<FlashcardCategoryWithLanguageIds>.Success(result);

    }
}
