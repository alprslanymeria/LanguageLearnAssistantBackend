using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.DeckWords.Commands.UpdateDeckWord;

/// <summary>
/// HANDLER FOR UPDATE DECK WORD COMMAND.
/// </summary>
public class UpdateDeckWordCommandHandler(

    IDeckWordRepository deckWordRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<UpdateDeckWordCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        UpdateDeckWordCommand request,
        CancellationToken cancellationToken)
    {

        // VERIFY DECK WORD EXISTS
        var existingDeckWord = await deckWordRepository.GetByIdAsync(request.Request.ItemId)
            ?? throw new NotFoundException("DECK WORD NOT FOUND");

        // UPDATE OTHER FIELDS
        existingDeckWord.FlashcardCategoryId = request.Request.CategoryId;
        existingDeckWord.Question = request.Request.Word;
        existingDeckWord.Answer = request.Request.Answer;

        // UPDATE IN DB AND COMMIT
        deckWordRepository.Update(existingDeckWord);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);

        return ServiceResult.Success();
    }
}
