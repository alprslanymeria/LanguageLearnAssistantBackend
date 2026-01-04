using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.DeckWords.Commands.DeleteDWordItemById;

/// <summary>
/// HANDLER FOR DELETE DECK WORD BY ID COMMAND.
/// </summary>
public class DeleteDWordItemByIdCommandHandler(

    IDeckWordRepository deckWordRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteDWordItemByIdCommandHandler> logger,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<DeleteDWordItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        DeleteDWordItemByIdCommand request, 
        CancellationToken cancellationToken)
    {
        var id = request.Id;

        logger.LogInformation("DeleteDWordItemByIdCommandHandler -> ATTEMPTING TO DELETE DECK WORD WITH ID: {Id}", id);

        var deckWord = await deckWordRepository.GetByIdAsync(id);

        // FAST FAIL
        if (deckWord is null)
        {
            logger.LogWarning("DeleteDWordItemByIdCommandHandler -> DECK WORD NOT FOUND FOR DELETION WITH ID: {Id}", id);
            return ServiceResult.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        deckWordRepository.Delete(deckWord);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);

        logger.LogInformation("DeleteDWordItemByIdCommandHandler -> SUCCESSFULLY DELETED DECK WORD FROM DATABASE WITH ID: {Id}", id);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
