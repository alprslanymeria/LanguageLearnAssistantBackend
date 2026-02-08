using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.WritingSessionRows.Commands.CreateWRows;

public class CreateWRowsCommandHandler(

    IWritingSessionRowRepository writingSessionRowRepository,
    IWritingOldSessionRepository writingOldSessionRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateWRowsCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateWRowsCommand request,
        CancellationToken cancellationToken)
    {

        // GET OLD SESSION
        var session = await writingOldSessionRepository.GetByIdAsync(request.Request.WritingOldSessionId)
            ?? throw new NotFoundException("WRITING OLD SESSION NOT FOUND");

        var rows = request.Request.Rows.Select(r => new WritingSessionRow
        {
            WritingOldSessionId = request.Request.WritingOldSessionId,
            SelectedSentence = r.SelectedSentence,
            Answer = r.Answer,
            AnswerTranslate = r.AnswerTranslate,
            Similarity = r.Similarity,
            WritingOldSession = session

        }).ToList();

        await writingSessionRowRepository.AddRangeAsync(rows);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
