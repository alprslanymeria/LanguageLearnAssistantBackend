using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingOldSessions.Commands.CreateROS;

/// <summary>
/// HANDLER FOR SAVE READING OLD SESSION COMMAND.
/// </summary>
public class CreateROSCommandHandler(

    IReadingOldSessionRepository readingOldSessionRepository,
    IReadingRepository readingRepository,
    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateROSCommandHandler> logger

    ) : ICommandHandler<CreateROSCommand, ServiceResult<string>>
{
    public async Task<ServiceResult<string>> Handle(

        CreateROSCommand request, 
        CancellationToken cancellationToken)
    {

        logger.LogInformation("CreateROSCommandHandler -> SAVING READING OLD SESSION WITH ID: {SessionId}", request.Request.Id);

        var reading = await readingRepository.GetByIdAsync(request.Request.ReadingId);

        // FAST FAIL
        if (reading is null)
        {
            logger.LogWarning("CreateROSCommandHandler -> READING NOT FOUND WITH ID: {ReadingId}", request.Request.ReadingId);
            return ServiceResult<string>.Fail("READING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var readingBook = await readingBookRepository.GetByIdAsync(request.Request.ReadingBookId);

        // FAST FAIL
        if (readingBook is null)
        {
            logger.LogWarning("CreateROSCommandHandler -> READING BOOK NOT FOUND WITH ID: {ReadingBookId}", request.Request.ReadingBookId);
            return ServiceResult<string>.Fail("READING BOOK NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new ReadingOldSession
        {
            Id = request.Request.Id,
            ReadingId = request.Request.ReadingId,
            ReadingBookId = request.Request.ReadingBookId,
            Rate = request.Request.Rate,
            CreatedAt = DateTime.UtcNow,
            Reading = reading,
            ReadingBook = readingBook
        };

        await readingOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateROSCommandHandler -> SUCCESSFULLY SAVED READING OLD SESSION WITH ID: {SessionId}", session.Id);

        return ServiceResult<string>.SuccessAsCreated(session.Id, $"/api/ReadingOldSession/{session.Id}");
    }
}
