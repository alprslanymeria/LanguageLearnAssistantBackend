using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardCategories.Commands.CreateFlashcardCategory;

/// <summary>
/// HANDLER FOR CREATE FLASHCARD CATEGORY COMMAND.
/// </summary>
public class CreateFlashcardCategoryCommandHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IPracticeRepository practiceRepository

    ) : ICommandHandler<CreateFlashcardCategoryCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        CreateFlashcardCategoryCommand request,
        CancellationToken cancellationToken)
    {

        // GET PRACTICE
        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Request.Practice, request.Request.LanguageId)
            ?? throw new NotFoundException("PRACTICE NOT FOUND");

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

        // CREATE FLASHCARD CATEGORY
        var flashcardCategory = new FlashcardCategory
        {
            Name = request.Request.CategoryName,
            FlashcardId = flashcard.Id
        };

        // ADD FLASHCARD CATEGORY TO DB AND COMMIT
        await flashcardCategoryRepository.AddAsync(flashcardCategory);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
