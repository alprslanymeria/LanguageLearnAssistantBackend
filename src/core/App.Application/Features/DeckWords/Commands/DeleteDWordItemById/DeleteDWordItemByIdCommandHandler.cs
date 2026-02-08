using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.DeckWords.Commands.DeleteDWordItemById;

/// <summary>
/// HANDLER FOR DELETE DECK WORD BY ID COMMAND.
/// </summary>
public class DeleteDWordItemByIdCommandHandler(

    IDeckWordRepository deckWordRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<DeleteDWordItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        DeleteDWordItemByIdCommand request,
        CancellationToken cancellationToken)
    {

        // GET DECKWORD
        var deckWord = await deckWordRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("DECK WORD NOT FOUND");

        // REMOVE DECK WORD AND COMMIT
        await deckWordRepository.RemoveAsync(deckWord.Id);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
