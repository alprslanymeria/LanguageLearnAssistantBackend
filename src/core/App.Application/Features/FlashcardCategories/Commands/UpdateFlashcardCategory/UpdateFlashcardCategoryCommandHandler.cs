using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardCategories.Commands.UpdateFlashcardCategory;

/// <summary>
/// HANDLER FOR UPDATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class UpdateFlashcardCategoryCommandHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IPracticeRepository practiceRepository

    ) : ICommandHandler<UpdateFlashcardCategoryCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        UpdateFlashcardCategoryCommand request,
        CancellationToken cancellationToken)
    {

        // GET PRACTICE
        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Request.Practice, request.Request.LanguageId)
            ?? throw new NotFoundException("PRACTICE NOT FOUND");

        // VERIFY FLASHCARD CATEGORY EXISTS
        var existingCategory = await flashcardCategoryRepository.GetByIdAsync(request.Request.ItemId)
            ?? throw new NotFoundException("FLASHCARD CATEGORY NOT FOUND");

        // VERIFY OR CREATE FLASHCARD
        var flashcardResult = await entityVerificationService.VerifyOrCreateFlashcardAsync(

            practice.Id,
            request.Request.UserId,
            request.Request.LanguageId);

        if (flashcardResult.IsFail)
        {
            throw new BusinessException(flashcardResult.ErrorMessage!.First(), flashcardResult.Status);
        }

        var flashcard = flashcardResult.Data!;

        // UPDATE OTHER FIELDS
        existingCategory.FlashcardId = flashcard.Id;
        existingCategory.Name = request.Request.CategoryName;

        flashcardCategoryRepository.Update(existingCategory);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);

        return ServiceResult.Success();
    }
}
