using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardCategories.Commands.DeleteFCategoryItemById;

/// <summary>
/// HANDLER FOR DELETE FLASHCARD CATEGORY BY ID COMMAND.
/// </summary>
public class DeleteFCategoryItemByIdCommandHandler(
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteFCategoryItemByIdCommandHandler> logger
    ) : ICommandHandler<DeleteFCategoryItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        DeleteFCategoryItemByIdCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteFCategoryItemByIdCommandHandler -> ATTEMPTING TO DELETE FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.Id);

        if (flashcardCategory is null)
        {
            logger.LogWarning("DeleteFCategoryItemByIdCommandHandler -> FLASHCARD CATEGORY NOT FOUND FOR DELETION WITH ID: {Id}", request.Id);
            return ServiceResult.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        flashcardCategoryRepository.Delete(flashcardCategory);
        await unitOfWork.CommitAsync();

        logger.LogInformation("DeleteFCategoryItemByIdCommandHandler -> SUCCESSFULLY DELETED FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
