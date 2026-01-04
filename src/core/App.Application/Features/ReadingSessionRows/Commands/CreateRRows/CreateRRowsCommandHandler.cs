using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingSessionRows.Commands.CreateRRows;

public class CreateRRowsCommandHandler(

    IReadingSessionRowRepository readingSessionRowRepository,
    IReadingOldSessionRepository readingOldSessionRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateRRowsCommandHandler> logger

    ) : ICommandHandler<CreateRRowsCommand, ServiceResult<int>>
{
    public async Task<ServiceResult<int>> Handle(

        CreateRRowsCommand request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("CreateRRowsCommandHandler -> SAVING {Count} READING ROWS FOR SESSION: {SessionId}", request.Request.Rows.Count, request.Request.ReadingOldSessionId);

        var session = await readingOldSessionRepository.GetByIdAsync(request.Request.ReadingOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("CreateRRowsCommandHandler -> READING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.Request.ReadingOldSessionId);
            return ServiceResult<int>.Fail("READING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Request.Rows.Select(r => new ReadingSessionRow
        {
            ReadingOldSessionId = request.Request.ReadingOldSessionId,
            SelectedSentence = r.SelectedSentence,
            Answer = r.Answer,
            AnswerTranslate = r.AnswerTranslate,
            Similarity = r.Similarity,
            ReadingOldSession = session

        }).ToList();

        await readingSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateRRowsCommandHandler -> SUCCESSFULLY SAVED {Count} READING ROWS FOR SESSION: {SessionId}", rows.Count, request.Request.ReadingOldSessionId);

        return ServiceResult<int>.Success(rows.Count, HttpStatusCode.Created);
    }
}
