using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ListeningOldSessions.Commands.CreateLOS;

public class CreateLOSCommandHandler(

    IListeningOldSessionRepository listeningOldSessionRepository,
    IListeningRepository listeningRepository,
    IListeningCategoryRepository listeningCategoryRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateLOSCommandHandler> logger

    ) : ICommandHandler<CreateLOSCommand, ServiceResult<string>>
{
    public async Task<ServiceResult<string>> Handle(

        CreateLOSCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateLOSCommandHandler -> SAVING LISTENING OLD SESSION WITH ID: {SessionId}", request.Request.Id);

        var listening = await listeningRepository.GetByIdAsync(request.Request.ListeningId);

        // FAST FAIL
        if (listening is null)
        {
            logger.LogWarning("CreateLOSCommandHandler -> LISTENING NOT FOUND WITH ID: {ListeningId}", request.Request.ListeningId);
            return ServiceResult<string>.Fail("LISTENING NOT FOUND.", HttpStatusCode.NotFound);
        }

        var listeningCategory = await listeningCategoryRepository.GetByIdAsync(request.Request.ListeningCategoryId);

        // FAST FAIL
        if (listeningCategory is null)
        {
            logger.LogWarning("CreateLOSCommandHandler -> LISTENING CATEGORY NOT FOUND WITH ID: {ListeningCategoryId}", request.Request.ListeningCategoryId);
            return ServiceResult<string>.Fail("LISTENING CATEGORY NOT FOUND.", HttpStatusCode.NotFound);
        }

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

        await listeningOldSessionRepository.CreateAsync(session);
        await unitOfWork.CommitAsync();

        logger.LogInformation("CreateLOSCommandHandler -> SUCCESSFULLY SAVED LISTENING OLD SESSION WITH ID: {SessionId}", session.Id);

        return ServiceResult<string>.SuccessAsCreated(session.Id, $"/api/ListeningOldSession/{session.Id}");

    }
}
