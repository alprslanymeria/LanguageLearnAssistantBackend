using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.WritingOldSessions.Commands.CreateWOS;

public class CreateWOSCommandHandler(

    IWritingOldSessionRepository writingOldSessionRepository,
    IWritingRepository writingRepository,
    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateWOSCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateWOSCommand request,
        CancellationToken cancellationToken)
    {

        // GET WRITING
        var writing = await writingRepository.GetByIdAsync(request.Request.WritingId)
            ?? throw new NotFoundException("WRITING NOT FOUND.");

        // GET WRITING BOOK
        var writingBook = await writingBookRepository.GetByIdAsync(request.Request.WritingBookId)
            ?? throw new NotFoundException("WRITING BOOK NOT FOUND.");

        var session = new WritingOldSession
        {
            Id = request.Request.Id,
            Rate = request.Request.Rate,
            CreatedAt = DateTime.UtcNow,
            WritingId = request.Request.WritingId,
            WritingBookId = request.Request.WritingBookId
        };

        await writingOldSessionRepository.AddAsync(session);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
