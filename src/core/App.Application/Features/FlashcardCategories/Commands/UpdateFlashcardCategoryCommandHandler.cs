using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardCategories.Commands;

/// <summary>
/// HANDLER FOR UPDATING AN EXISTING FLASHCARD CATEGORY.
/// </summary>
public class UpdateFlashcardCategoryCommandHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IFlashcardRepository flashcardRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateFlashcardCategoryCommandHandler> logger)
    : ICommandHandler<UpdateFlashcardCategoryCommand, FlashcardCategoryDto>
{
    public async Task<ServiceResult<FlashcardCategoryDto>> Handle(
        UpdateFlashcardCategoryCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateFlashcardCategoryCommandHandler: UPDATING FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        // VERIFY FLASHCARD CATEGORY EXISTS
        var existingCategory = await flashcardCategoryRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (existingCategory is null)
        {
            logger.LogWarning("UpdateFlashcardCategoryCommandHandler: FLASHCARD CATEGORY NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<FlashcardCategoryDto>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE FLASHCARD
        var flashcardResult = await VerifyOrCreateFlashcardAsync(request.FlashcardId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!flashcardResult.IsSuccess)
        {
            return ServiceResult<FlashcardCategoryDto>.Fail(flashcardResult.ErrorMessage!, flashcardResult.Status);
        }

        var flashcard = flashcardResult.Data!;

        try
        {
            // UPDATE FIELDS
            existingCategory.FlashcardId = flashcard.Id;
            existingCategory.Name = request.Name;

            flashcardCategoryRepository.Update(existingCategory);
            await unitOfWork.CommitAsync();

            logger.LogInformation("UpdateFlashcardCategoryCommandHandler: SUCCESSFULLY UPDATED FLASHCARD CATEGORY WITH ID: {Id}", existingCategory.Id);

            var result = mapper.Map<FlashcardCategory, FlashcardCategoryDto>(existingCategory);
            return ServiceResult<FlashcardCategoryDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateFlashcardCategoryCommandHandler: ERROR UPDATING FLASHCARD CATEGORY");
            return ServiceResult<FlashcardCategoryDto>.Fail("ERROR UPDATING FLASHCARD CATEGORY", HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// VERIFY OR CREATE FLASHCARD IF IT DOESN'T EXIST
    /// </summary>
    private async Task<ServiceResult<Flashcard>> VerifyOrCreateFlashcardAsync(int flashcardId, string userId, int languageId)
    {
        var flashcard = await flashcardRepository.GetByIdAsync(flashcardId);

        if (flashcard is not null)
        {
            return ServiceResult<Flashcard>.Success(flashcard);
        }

        logger.LogWarning("UpdateFlashcardCategoryCommandHandler: FLASHCARD NOT FOUND WITH ID: {FlashcardId}", flashcardId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("UpdateFlashcardCategoryCommandHandler: LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("UpdateFlashcardCategoryCommandHandler: PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        flashcard = new Flashcard
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id,
            Language = language,
            Practice = practice
        };

        await flashcardRepository.CreateAsync(flashcard);

        logger.LogInformation("UpdateFlashcardCategoryCommandHandler: NEW FLASHCARD CREATED WITH ID: {FlashcardId}", flashcard.Id);

        return ServiceResult<Flashcard>.Success(flashcard);
    }
}
