using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.FlashcardSessionRows.Commands.CreateFRows;

public class CreateFRowsCommandHandler(

    IFlashcardSessionRowRepository flashcardSessionRowRepository,
    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateFRowsCommandHandler> logger

    ) : ICommandHandler<CreateFRowsCommand, ServiceResult<int>>
{
    public async Task<ServiceResult<int>> Handle(

        CreateFRowsCommand request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("CreateFRowsCommandHandler -> SAVING {Count} FLASHCARD ROWS FOR SESSION: {SessionId}", request.Request.Rows.Count, request.Request.FlashcardOldSessionId);

        var session = await flashcardOldSessionRepository.GetByIdAsync(request.Request.FlashcardOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("CreateFRowsCommandHandler -> FLASHCARD OLD SESSION NOT FOUND WITH ID: {SessionId}", request.Request.FlashcardOldSessionId);
            return ServiceResult<int>.Fail("FLASHCARD OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Request.Rows.Select(r => new FlashcardSessionRow
        {
            FlashcardOldSessionId = request.Request.FlashcardOldSessionId,
            Answer = r.Answer,
            Question = r.Question,
            Status = r.Status,
            FlashcardOldSession = session

        }).ToList();

        await flashcardSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateFRowsCommandHandler -> SUCCESSFULLY SAVED {Count} FLASHCARD ROWS FOR SESSION: {SessionId}", rows.Count, request.Request.FlashcardOldSessionId);

        return ServiceResult<int>.Success(rows.Count, HttpStatusCode.Created);

    }
}
