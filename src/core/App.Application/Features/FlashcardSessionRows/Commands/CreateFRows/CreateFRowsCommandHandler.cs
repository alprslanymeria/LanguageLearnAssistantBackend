using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.FlashcardSessionRows.Commands.CreateFRows;

public class CreateFRowsCommandHandler(

    IFlashcardSessionRowRepository flashcardSessionRowRepository,
    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateFRowsCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateFRowsCommand request,
        CancellationToken cancellationToken)
    {

        // GET OLDSESSION
        var session = await flashcardOldSessionRepository.GetByIdAsync(request.Request.FlashcardOldSessionId)
            ?? throw new NotFoundException("FLASHCARD OLD SESSION NOT FOUND");

        var rows = request.Request.Rows.Select(r => new FlashcardSessionRow
        {
            Answer = r.Answer,
            Question = r.Question,
            Status = r.Status,
            FlashcardOldSessionId = request.Request.FlashcardOldSessionId

        }).ToList();

        await flashcardSessionRowRepository.AddRangeAsync(rows);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);

    }
}
