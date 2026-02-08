using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.ReadingOldSessions.Commands.CreateROS;

/// <summary>
/// HANDLER FOR SAVE READING OLD SESSION COMMAND.
/// </summary>
public class CreateROSCommandHandler(

    IReadingOldSessionRepository readingOldSessionRepository,
    IReadingRepository readingRepository,
    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateROSCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateROSCommand request,
        CancellationToken cancellationToken)
    {

        // GET READING
        var reading = await readingRepository.GetByIdAsync(request.Request.ReadingId)
            ?? throw new NotFoundException("READING NOT FOUND.");

        // GET READING BOOK
        var readingBook = await readingBookRepository.GetByIdAsync(request.Request.ReadingBookId)
            ?? throw new NotFoundException("READING BOOK NOT FOUND.");

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

        await readingOldSessionRepository.AddAsync(session);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
