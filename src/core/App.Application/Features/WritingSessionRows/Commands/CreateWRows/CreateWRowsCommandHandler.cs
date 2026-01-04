using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingSessionRows.Commands.CreateWRows;

public class CreateWRowsCommandHandler(

    IWritingSessionRowRepository writingSessionRowRepository,
    IWritingOldSessionRepository writingOldSessionRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateWRowsCommandHandler> logger

    ) : ICommandHandler<CreateWRowsCommand, ServiceResult<int>>
{
    public async Task<ServiceResult<int>> Handle(

        CreateWRowsCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateWRowsCommandHandler -> SAVING {Count} WRITING ROWS FOR SESSION: {SessionId}", request.Request.Rows.Count, request.Request.WritingOldSessionId);

        var session = await writingOldSessionRepository.GetByIdAsync(request.Request.WritingOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("CreateWRowsCommandHandler -> WRITING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.Request.WritingOldSessionId);
            return ServiceResult<int>.Fail("WRITING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Request.Rows.Select(r => new WritingSessionRow
        {
            WritingOldSessionId = request.Request.WritingOldSessionId,
            SelectedSentence = r.SelectedSentence,
            Answer = r.Answer,
            AnswerTranslate = r.AnswerTranslate,
            Similarity = r.Similarity,
            WritingOldSession = session

        }).ToList();

        await writingSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateWRowsCommandHandler -> SUCCESSFULLY SAVED {Count} WRITING ROWS FOR SESSION: {SessionId}", rows.Count, request.Request.WritingOldSessionId);

        return ServiceResult<int>.Success(rows.Count, HttpStatusCode.Created);
    }
}
