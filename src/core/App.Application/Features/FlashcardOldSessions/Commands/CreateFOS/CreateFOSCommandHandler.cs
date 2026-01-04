using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardOldSessions.Commands.CreateFOS;

public class CreateFOSCommandHandler(

    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IFlashcardRepository flashcardRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateFOSCommandHandler> logger

    ) : ICommandHandler<CreateFOSCommand, ServiceResult<string>>
{
    public async Task<ServiceResult<string>> Handle(

        CreateFOSCommand request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("CreateFOSCommandHandler -> SAVING FLASHCARD OLD SESSION WITH ID: {SessionId}", request.Request.Id);

        var flashcard = await flashcardRepository.GetByIdAsync(request.Request.FlashcardId);

        // FAST FAIL
        if (flashcard is null)
        {
            logger.LogWarning("CreateFOSCommandHandler -> FLASHCARD NOT FOUND WITH ID: {FlashcardId}", request.Request.FlashcardId);
            return ServiceResult<string>.Fail("FLASHCARD NOT FOUND.", HttpStatusCode.NotFound);
        }

        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.Request.FlashcardCategoryId);

        // FAST FAIL
        if (flashcardCategory is null)
        {
            logger.LogWarning("CreateFOSCommandHandler -> FLASHCARD CATEGORY NOT FOUND WITH ID: {FlashcardCategoryId}", request.Request.FlashcardCategoryId);
            return ServiceResult<string>.Fail("FLASHCARD CATEGORY NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new FlashcardOldSession
        {
            Id = request.Request.Id,
            FlashcardId = request.Request.FlashcardId,
            FlashcardCategoryId = request.Request.FlashcardCategoryId,
            Rate = request.Request.Rate,
            CreatedAt = DateTime.UtcNow,
            Flashcard = flashcard,
            FlashcardCategory = flashcardCategory
        };

        await flashcardOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateFOSCommandHandler -> SUCCESSFULLY SAVED FLASHCARD OLD SESSION WITH ID: {SessionId}", session.Id);

        return ServiceResult<string>.SuccessAsCreated(session.Id, $"/api/FlashcardOldSession/{session.Id}");
    }
}
