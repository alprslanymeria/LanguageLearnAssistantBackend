using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ListeningSessionRows.Commands.CreateLRows;

public class CreateLRowsCommandHandler(

    IListeningSessionRowRepository listeningSessionRowRepository,
    IListeningOldSessionRepository listeningOldSessionRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateLRowsCommandHandler> logger

    ) : ICommandHandler<CreateLRowsCommand, ServiceResult<int>>
{
    public async Task<ServiceResult<int>> Handle(

        CreateLRowsCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateLRowsCommandHandler -> SAVING {Count} LISTENING ROWS FOR SESSION: {SessionId}", request.Request.Rows.Count, request.Request.ListeningOldSessionId);

        var session = await listeningOldSessionRepository.GetByIdAsync(request.Request.ListeningOldSessionId);

        // FAST FAIL
        if (session is null)
        {
            logger.LogWarning("CreateLRowsCommandHandler -> LISTENING OLD SESSION NOT FOUND WITH ID: {SessionId}", request.Request.ListeningOldSessionId);
            return ServiceResult<int>.Fail("LISTENING OLD SESSION NOT FOUND", HttpStatusCode.NotFound);
        }

        var rows = request.Request.Rows.Select(r => new ListeningSessionRow
        {
            ListeningOldSessionId = request.Request.ListeningOldSessionId,
            ListenedSentence = r.ListenedSentence,
            Answer = r.Answer,
            Similarity = r.Similarity,
            ListeningOldSession = session

        }).ToList();

        await listeningSessionRowRepository.CreateRangeAsync(rows);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateLRowsCommandHandler -> SUCCESSFULLY SAVED {Count} LISTENING ROWS FOR SESSION: {SessionId}", rows.Count, request.Request.ListeningOldSessionId);

        return ServiceResult<int>.Success(rows.Count, HttpStatusCode.Created);
    }
}
