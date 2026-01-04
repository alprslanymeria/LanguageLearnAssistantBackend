using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardCategories.Queries.GetFlashcardCategoryById;

/// <summary>
/// HANDLER FOR GET FLASHCARD CATEGORY BY ID QUERY.
/// </summary>
public class GetFlashcardCategoryByIdQueryHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IMapper mapper,
    ILogger<GetFlashcardCategoryByIdQueryHandler> logger

    ) : IQueryHandler<GetFlashcardCategoryByIdQuery, ServiceResult<FlashcardCategoryWithLanguageId>>
{
    public async Task<ServiceResult<FlashcardCategoryWithLanguageId>> Handle(

        GetFlashcardCategoryByIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetFlashcardCategoryByIdQueryHandler --> FETCHING FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        var flashcardCategory = await flashcardCategoryRepository.GetFlashcardCategoryItemByIdAsync(request.Id);

        if (flashcardCategory is null)
        {
            logger.LogWarning("GetFlashcardCategoryByIdQueryHandler --> FLASHCARD CATEGORY NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<FlashcardCategoryWithLanguageId>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("GetFlashcardCategoryByIdQueryHandler --> SUCCESSFULLY FETCHED FLASHCARD CATEGORY: {CategoryName}", flashcardCategory.Name);

        var result = mapper.Map<FlashcardCategory, FlashcardCategoryWithLanguageId>(flashcardCategory);
        return ServiceResult<FlashcardCategoryWithLanguageId>.Success(result);
    }
}
