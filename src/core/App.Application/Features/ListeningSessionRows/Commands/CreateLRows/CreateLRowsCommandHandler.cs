using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.ListeningSessionRows.Commands.CreateLRows;

public class CreateLRowsCommandHandler(

    IListeningSessionRowRepository listeningSessionRowRepository,
    IListeningOldSessionRepository listeningOldSessionRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateLRowsCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateLRowsCommand request,
        CancellationToken cancellationToken)
    {
        // GET OLD SESSION
        var session = await listeningOldSessionRepository.GetByIdAsync(request.Request.ListeningOldSessionId)
            ?? throw new NotFoundException("LISTENING OLD SESSION NOT FOUND");

        var rows = request.Request.Rows.Select(r => new ListeningSessionRow
        {
            ListenedSentence = r.ListenedSentence,
            Answer = r.Answer,
            Similarity = r.Similarity,
            ListeningOldSessionId = request.Request.ListeningOldSessionId

        }).ToList();

        await listeningSessionRowRepository.AddRangeAsync(rows);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
