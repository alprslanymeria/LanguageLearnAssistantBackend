using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.DeckWords.Commands.DeleteDWordItemById;

/// <summary>
/// HANDLER FOR DELETE DECK WORD BY ID COMMAND.
/// </summary>
public class DeleteDWordItemByIdCommandHandler(
    IDeckWordRepository deckWordRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteDWordItemByIdCommandHandler> logger
    ) : ICommandHandler<DeleteDWordItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        DeleteDWordItemByIdCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteDWordItemByIdCommandHandler -> ATTEMPTING TO DELETE DECK WORD WITH ID: {Id}", request.Id);

        var deckWord = await deckWordRepository.GetByIdAsync(request.Id);

        if (deckWord is null)
        {
            logger.LogWarning("DeleteDWordItemByIdCommandHandler -> DECK WORD NOT FOUND FOR DELETION WITH ID: {Id}", request.Id);
            return ServiceResult.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        deckWordRepository.Delete(deckWord);
        await unitOfWork.CommitAsync();

        logger.LogInformation("DeleteDWordItemByIdCommandHandler -> SUCCESSFULLY DELETED DECK WORD WITH ID: {Id}", request.Id);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
