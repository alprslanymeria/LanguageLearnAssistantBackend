using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.ListeningOldSessions.Commands.CreateLOS;

public class CreateLOSCommandHandler(

    IListeningOldSessionRepository listeningOldSessionRepository,
    IListeningRepository listeningRepository,
    IListeningCategoryRepository listeningCategoryRepository,
    IUnitOfWork unitOfWork

    ) : ICommandHandler<CreateLOSCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateLOSCommand request,
        CancellationToken cancellationToken)
    {

        // GET LISTENING
        var listening = await listeningRepository.GetByIdAsync(request.Request.ListeningId)
            ?? throw new NotFoundException("LISTENING NOT FOUND.");

        // GET LISTENING CATEGORY
        var listeningCategory = await listeningCategoryRepository.GetByIdAsync(request.Request.ListeningCategoryId)
            ?? throw new NotFoundException("LISTENING CATEGORY NOT FOUND.");

        var session = new ListeningOldSession
        {
            Id = request.Request.Id,
            ListeningId = request.Request.ListeningId,
            ListeningCategoryId = request.Request.ListeningCategoryId,
            Rate = request.Request.Rate,
            CreatedAt = DateTime.UtcNow,
            Listening = listening,
            ListeningCategory = listeningCategory
        };

        await listeningOldSessionRepository.AddAsync(session);
        await unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.Created);

    }
}
