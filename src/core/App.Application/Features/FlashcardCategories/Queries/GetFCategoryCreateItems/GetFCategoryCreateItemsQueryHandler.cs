using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardCategories.Queries.GetFCategoryCreateItems;

/// <summary>
/// HANDLER FOR GET FLASHCARD CATEGORY CREATE ITEMS QUERY.
/// </summary>
public class GetFCategoryCreateItemsQueryHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository

    ) : IQueryHandler<GetFCategoryCreateItemsQuery, ServiceResult<List<FlashcardCategoryWithDeckWords>>>
{
    public async Task<ServiceResult<List<FlashcardCategoryWithDeckWords>>> Handle(

        GetFCategoryCreateItemsQuery request,
        CancellationToken cancellationToken)
    {

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language)
            ?? throw new NotFoundException($"LANGUAGE '{request.Language}' NOT FOUND");

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id)
            ?? throw new NotFoundException($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.");

        var flashcardCategories = await flashcardCategoryRepository.GetFCategoryCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        return ServiceResult<List<FlashcardCategoryWithDeckWords>>.Success(flashcardCategories);
    }
}
