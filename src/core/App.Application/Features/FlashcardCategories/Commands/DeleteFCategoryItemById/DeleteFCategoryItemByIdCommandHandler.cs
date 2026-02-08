using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardCategories.Commands.DeleteFCategoryItemById;

/// <summary>
/// HANDLER FOR DELETE FLASHCARD CATEGORY BY ID COMMAND.
/// </summary>
public class DeleteFCategoryItemByIdCommandHandler(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<DeleteFCategoryItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        DeleteFCategoryItemByIdCommand request,
        CancellationToken cancellationToken)
    {

        // GET FLASHCARD CATEGORY
        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("FLASHCARD CATEGORY NOT FOUND");

        // REMOVE FLASHCARD CATEGORY AND COMMIT
        await flashcardCategoryRepository.RemoveAsync(flashcardCategory.Id);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(FlashcardCategoryCacheKeys.Prefix);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
