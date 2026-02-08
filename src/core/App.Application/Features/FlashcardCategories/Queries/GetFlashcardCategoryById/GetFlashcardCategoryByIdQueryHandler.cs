using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardCategories.Queries.GetFlashcardCategoryById;

/// <summary>
/// HANDLER FOR GET FLASHCARD CATEGORY BY ID QUERY.
/// </summary>
public class GetFlashcardCategoryByIdQueryHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository

    ) : IQueryHandler<GetFlashcardCategoryByIdQuery, ServiceResult<FlashcardCategoryWithLanguageId>>
{
    public async Task<ServiceResult<FlashcardCategoryWithLanguageId>> Handle(

        GetFlashcardCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {

        // GET FLASHCARD CATEGORY
        var flashcardCategory = await flashcardCategoryRepository.GetFlashcardCategoryItemByIdAsync(request.Id)
            ?? throw new NotFoundException("FLASHCARD CATEGORY NOT FOUND");

        return ServiceResult<FlashcardCategoryWithLanguageId>.Success(flashcardCategory);
    }
}
