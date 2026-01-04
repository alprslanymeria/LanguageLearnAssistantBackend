using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingOldSessions.Commands.CreateWOS;

public class CreateWOSCommandHandler(

    IWritingOldSessionRepository writingOldSessionRepository,
    IWritingRepository writingRepository,
    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateWOSCommandHandler> logger

    ) : ICommandHandler<CreateWOSCommand, ServiceResult<string>>
{
    public async Task<ServiceResult<string>> Handle(

        CreateWOSCommand request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("CreateWOSCommandHandler -> SAVING WRITING OLD SESSION WITH ID: {SessionId}", request.Request.Id);

        var writing = await writingRepository.GetByIdAsync(request.Request.WritingId);

        // FAST FAIL
        if (writing is null)
        {
            logger.LogWarning("CreateWOSCommandHandler -> WRITING NOT FOUND WITH ID: {WritingId}", request.Request.WritingId);
            return ServiceResult<string>.Fail("WRITING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var writingBook = await writingBookRepository.GetByIdAsync(request.Request.WritingBookId);

        // FAST FAIL
        if (writingBook is null)
        {
            logger.LogWarning("CreateWOSCommandHandler -> WRITING BOOK NOT FOUND WITH ID: {WritingBookId}", request.Request.WritingBookId);
            return ServiceResult<string>.Fail("WRITING BOOK NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new WritingOldSession
        {
            Id = request.Request.Id,
            WritingId = request.Request.WritingId,
            WritingBookId = request.Request.WritingBookId,
            Rate = request.Request.Rate,
            CreatedAt = DateTime.UtcNow,
            Writing = writing,
            WritingBook = writingBook
        };

        await writingOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateWOSCommandHandler -> SUCCESSFULLY SAVED WRITING OLD SESSION WITH ID: {SessionId}", session.Id);

        return ServiceResult<string>.SuccessAsCreated(session.Id, $"/api/WritingOldSession/{session.Id}");
    }
}
