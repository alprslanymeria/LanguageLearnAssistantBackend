using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardCategories.Commands.CreateFlashcardCategory;

/// <summary>
/// HANDLER FOR CREATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class CreateFlashcardCategoryCommandHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateFlashcardCategoryCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService

    ) : ICommandHandler<CreateFlashcardCategoryCommand, ServiceResult<int>>
{

    public async Task<ServiceResult<int>> Handle(

        CreateFlashcardCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateFlashcardCategoryCommandHandler -> CREATING NEW FLASHCARD CATEGORY FOR FLASHCARD ID: {FlashcardId}", request.Request.FlashcardId);

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
            // CREATE FLASHCARD CATEGORY
            var flashcardCategory = new FlashcardCategory
            {
                FlashcardId = flashcard.Id,
                Name = request.Request.Name,
                Flashcard = flashcard
            };

            await flashcardCategoryRepository.CreateAsync(flashcardCategory);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);

            logger.LogInformation("CreateFlashcardCategoryCommandHandler -> SUCCESSFULLY CREATED FLASHCARD CATEGORY WITH ID: {Id}, NAME: {Name}", flashcardCategory.Id, flashcardCategory.Name);

            return ServiceResult<int>.SuccessAsCreated(flashcardCategory.Id, $"/api/FlashcardCategory/{flashcardCategory.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateFlashcardCategoryCommandHandler -> ERROR CREATING FLASHCARD CATEGORY");
            return ServiceResult<int>.Fail("ERROR CREATING FLASHCARD CATEGORY", HttpStatusCode.InternalServerError);
        }
    }
}
