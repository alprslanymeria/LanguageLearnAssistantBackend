using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardCategories.Queries.GetFCategoryCreateItems;

/// <summary>
/// HANDLER FOR GET FLASHCARD CATEGORY CREATE ITEMS QUERY.
/// </summary>
public class GetFCategoryCreateItemsQueryHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IMapper mapper,
    ILogger<GetFCategoryCreateItemsQueryHandler> logger

    ) : IQueryHandler<GetFCategoryCreateItemsQuery, ServiceResult<List<FlashcardCategoryDto>>>
{
    public async Task<ServiceResult<List<FlashcardCategoryDto>>> Handle(

        GetFCategoryCreateItemsQuery request, 
        CancellationToken cancellationToken)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            logger.LogWarning("GetFCategoryCreateItemsQueryHandler --> USER ID IS REQUIRED FOR FETCHING CREATE ITEMS");
            return ServiceResult<List<FlashcardCategoryDto>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("GetFCategoryCreateItemsQueryHandler --> FETCHING FLASHCARD CATEGORY CREATE ITEMS FOR USER: {UserId}", request.UserId);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetFCategoryCreateItemsQueryHandler --> LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND",
                HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("GetFCategoryCreateItemsQueryHandler --> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", request.Practice, request.Language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.",
                HttpStatusCode.NotFound);
        }

        var flashcardCategories = await flashcardCategoryRepository.GetFCategoryCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(flashcardCategories);

        logger.LogInformation("GetFCategoryCreateItemsQueryHandler -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", result.Count);

        return ServiceResult<List<FlashcardCategoryDto>>.Success(result);
    }
}
