using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingOldSessions.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.ReadingOldSessions.Commands.SaveReadingOldSession;

/// <summary>
/// HANDLER FOR SAVE READING OLD SESSION COMMAND.
/// </summary>
public class SaveReadingOldSessionCommandHandler(
    IReadingOldSessionRepository readingOldSessionRepository,
    IReadingRepository readingRepository,
    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<SaveReadingOldSessionCommandHandler> logger
    ) : ICommandHandler<SaveReadingOldSessionCommand, ServiceResult<ReadingOldSessionDto>>
{
    public async Task<ServiceResult<ReadingOldSessionDto>> Handle(
        SaveReadingOldSessionCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("SaveReadingOldSessionCommandHandler -> SAVING READING OLD SESSION WITH ID: {SessionId}", request.Id);

        var reading = await readingRepository.GetByIdAsync(request.ReadingId);

        if (reading is null)
        {
            logger.LogWarning("SaveReadingOldSessionCommandHandler -> READING NOT FOUND WITH ID: {ReadingId}", request.ReadingId);
            return ServiceResult<ReadingOldSessionDto>.Fail("READING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var readingBook = await readingBookRepository.GetByIdAsync(request.ReadingBookId);

        if (readingBook is null)
        {
            logger.LogWarning("SaveReadingOldSessionCommandHandler -> READING BOOK NOT FOUND WITH ID: {ReadingBookId}", request.ReadingBookId);
            return ServiceResult<ReadingOldSessionDto>.Fail("READING BOOK NOT FOUND.", HttpStatusCode.NotFound);
        }

        var session = new ReadingOldSession
        {
            Id = request.Id,
            ReadingId = request.ReadingId,
            ReadingBookId = request.ReadingBookId,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow,
            Reading = reading,
            ReadingBook = readingBook
        };

        await readingOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("SaveReadingOldSessionCommandHandler -> SUCCESSFULLY SAVED READING OLD SESSION WITH ID: {SessionId}", session.Id);

        var result = mapper.Map<ReadingOldSession, ReadingOldSessionDto>(session);
        return ServiceResult<ReadingOldSessionDto>.SuccessAsCreated(result, $"/api/ReadingOldSession/{session.Id}");
    }
}
