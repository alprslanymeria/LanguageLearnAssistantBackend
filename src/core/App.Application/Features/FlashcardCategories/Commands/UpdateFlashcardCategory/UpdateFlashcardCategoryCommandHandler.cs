using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.FlashcardCategories.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardCategories.Commands.UpdateFlashcardCategory;

/// <summary>
/// HANDLER FOR UPDATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class UpdateFlashcardCategoryCommandHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateFlashcardCategoryCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService

    ) : ICommandHandler<UpdateFlashcardCategoryCommand, ServiceResult<int>>
{

    public async Task<ServiceResult<int>> Handle(

        UpdateFlashcardCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateFlashcardCategoryCommandHandler -> UPDATING FLASHCARD CATEGORY WITH ID: {Id}", request.Request.Id);

        // VERIFY FLASHCARD CATEGORY EXISTS
        var existingCategory = await flashcardCategoryRepository.GetByIdAsync(request.Request.Id);

        // FAST FAIL
        if (existingCategory is null)
        {
            logger.LogWarning("UpdateFlashcardCategoryCommandHandler -> FLASHCARD CATEGORY NOT FOUND WITH ID: {Id}", request.Request.Id);
            return ServiceResult<int>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE FLASHCARD
        var flashcardResult = await entityVerificationService.VerifyOrCreateFlashcardAsync(

            request.Request.FlashcardId, 
            request.Request.UserId, 
            request.Request.LanguageId);

        // FAST FAIL
        if (!flashcardResult.IsSuccess)
        {
            return ServiceResult<int>.Fail(flashcardResult.ErrorMessage!, flashcardResult.Status);
        }

        var flashcard = flashcardResult.Data!;

        try
        {
            // UPDATE OTHER FIELDS
            existingCategory.FlashcardId = flashcard.Id;
            existingCategory.Name = request.Request.Name;

            flashcardCategoryRepository.Update(existingCategory);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);

            logger.LogInformation("UpdateFlashcardCategoryCommandHandler -> SUCCESSFULLY UPDATED FLASHCARD CATEGORY WITH ID: {Id}", existingCategory.Id);

            return ServiceResult<int>.Success(existingCategory.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateFlashcardCategoryCommandHandler -> ERROR UPDATING FLASHCARD CATEGORY");
            return ServiceResult<int>.Fail("ERROR UPDATING FLASHCARD CATEGORY", HttpStatusCode.InternalServerError);
        }
    }
}
