using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardOldSessions.Commands.CreateFOS;

public class CreateFOSCommandHandler(

    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IFlashcardRepository flashcardRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateFOSCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateFOSCommand request,
        CancellationToken cancellationToken)
    {

        // GET FLASHHCARD
        var flashcard = await flashcardRepository.GetByIdAsync(request.Request.FlashcardId)
            ?? throw new NotFoundException("FLASHCARD NOT FOUND.");

        // GET FLASHCARD CATEGORY
        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.Request.FlashcardCategoryId)
            ?? throw new NotFoundException("FLASHCARD CATEGORY NOT FOUND.");

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

        await flashcardOldSessionRepository.AddAsync(session);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
