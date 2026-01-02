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
/// HANDLER FOR CREATING A NEW FLASHCARD CATEGORY.
/// </summary>
public class CreateFlashcardCategoryCommandHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IFlashcardRepository flashcardRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateFlashcardCategoryCommandHandler> logger)
    : ICommandHandler<CreateFlashcardCategoryCommand, FlashcardCategoryDto>
{
    public async Task<ServiceResult<FlashcardCategoryDto>> Handle(
        CreateFlashcardCategoryCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateFlashcardCategoryCommandHandler: CREATING NEW FLASHCARD CATEGORY FOR FLASHCARD ID: {FlashcardId}", request.FlashcardId);

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
            // CREATE FLASHCARD CATEGORY
            var flashcardCategory = new FlashcardCategory
            {
                FlashcardId = flashcard.Id,
                Name = request.Name,
                Flashcard = flashcard
            };

            await flashcardCategoryRepository.CreateAsync(flashcardCategory);
            await unitOfWork.CommitAsync();

            logger.LogInformation("CreateFlashcardCategoryCommandHandler: SUCCESSFULLY CREATED FLASHCARD CATEGORY WITH ID: {Id}, NAME: {Name}",
                flashcardCategory.Id, flashcardCategory.Name);

            var result = mapper.Map<FlashcardCategory, FlashcardCategoryDto>(flashcardCategory);
            return ServiceResult<FlashcardCategoryDto>.SuccessAsCreated(result, $"/api/FlashcardCategory/{flashcardCategory.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateFlashcardCategoryCommandHandler: ERROR CREATING FLASHCARD CATEGORY");
            return ServiceResult<FlashcardCategoryDto>.Fail("ERROR CREATING FLASHCARD CATEGORY", HttpStatusCode.InternalServerError);
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

        logger.LogWarning("CreateFlashcardCategoryCommandHandler: FLASHCARD NOT FOUND WITH ID: {FlashcardId}", flashcardId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("CreateFlashcardCategoryCommandHandler: LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("CreateFlashcardCategoryCommandHandler: PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
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

        logger.LogInformation("CreateFlashcardCategoryCommandHandler: NEW FLASHCARD CREATED WITH ID: {FlashcardId}", flashcard.Id);

        return ServiceResult<Flashcard>.Success(flashcard);
    }
}
