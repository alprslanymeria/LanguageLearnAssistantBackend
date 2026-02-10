using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.ReadingSessionRows.Commands.CreateRRows;

public class CreateRRowsCommandHandler(

    IReadingSessionRowRepository readingSessionRowRepository,
    IReadingOldSessionRepository readingOldSessionRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateRRowsCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateRRowsCommand request,
        CancellationToken cancellationToken)
    {

        // GET SESSION
        var session = await readingOldSessionRepository.GetByIdAsync(request.Request.ReadingOldSessionId)
            ?? throw new NotFoundException("READING OLD SESSION NOT FOUND");

        var rows = request.Request.Rows.Select(r => new ReadingSessionRow
        {
            SelectedSentence = r.SelectedSentence,
            Answer = r.Answer,
            AnswerTranslate = r.AnswerTranslate,
            Similarity = r.Similarity,
            ReadingOldSessionId = request.Request.ReadingOldSessionId

        }).ToList();

        await readingSessionRowRepository.AddRangeAsync(rows);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
