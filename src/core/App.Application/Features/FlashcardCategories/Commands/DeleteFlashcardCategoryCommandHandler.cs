using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardCategories.Commands;

/// <summary>
/// HANDLER FOR DELETING A FLASHCARD CATEGORY BY ID.
/// </summary>
public class DeleteFlashcardCategoryCommandHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteFlashcardCategoryCommandHandler> logger)
    : ICommandHandler<DeleteFlashcardCategoryCommand>
{
    public async Task<ServiceResult> Handle(
        DeleteFlashcardCategoryCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteFlashcardCategoryCommandHandler: ATTEMPTING TO DELETE FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (flashcardCategory is null)
        {
            logger.LogWarning("DeleteFlashcardCategoryCommandHandler: FLASHCARD CATEGORY NOT FOUND FOR DELETION WITH ID: {Id}", request.Id);
            return ServiceResult.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        flashcardCategoryRepository.Delete(flashcardCategory);
        await unitOfWork.CommitAsync();

        logger.LogInformation("DeleteFlashcardCategoryCommandHandler: SUCCESSFULLY DELETED FLASHCARD CATEGORY FROM DATABASE WITH ID: {Id}", request.Id);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
