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
/// HANDLER FOR GETTING FLASHCARD CATEGORY CREATE ITEMS.
/// </summary>
public class GetFlashcardCategoryCreateItemsQueryHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IMapper mapper,
    ILogger<GetFlashcardCategoryCreateItemsQueryHandler> logger)
    : IQueryHandler<GetFlashcardCategoryCreateItemsQuery, List<FlashcardCategoryDto>>
{
    public async Task<ServiceResult<List<FlashcardCategoryDto>>> Handle(
        GetFlashcardCategoryCreateItemsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetFlashcardCategoryCreateItemsQueryHandler: FETCHING FLASHCARD CATEGORY CREATE ITEMS FOR USER: {UserId}", request.UserId);

        // CHECK IF LANGUAGE EXISTS
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetFlashcardCategoryCreateItemsQueryHandler: LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND",
                HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXISTS
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("GetFlashcardCategoryCreateItemsQueryHandler: PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}",
                request.Practice, request.Language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.",
                HttpStatusCode.NotFound);
        }

        var flashcardCategories = await flashcardCategoryRepository.GetFCategoryCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(flashcardCategories);

        logger.LogInformation("GetFlashcardCategoryCreateItemsQueryHandler: SUCCESSFULLY FETCHED {Count} CREATE ITEMS", result.Count);

        return ServiceResult<List<FlashcardCategoryDto>>.Success(result);
    }
}
