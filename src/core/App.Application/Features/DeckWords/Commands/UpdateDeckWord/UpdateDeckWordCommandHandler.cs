using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.DeckWords.Commands.UpdateDeckWord;

/// <summary>
/// HANDLER FOR UPDATE DECK WORD COMMAND.
/// </summary>
public class UpdateDeckWordCommandHandler(

    IDeckWordRepository deckWordRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateDeckWordCommandHandler> logger,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<UpdateDeckWordCommand, ServiceResult<int>>
{
    public async Task<ServiceResult<int>> Handle(

        UpdateDeckWordCommand request, 
        CancellationToken cancellationToken)
    {

        logger.LogInformation("UpdateDeckWordCommandHandler -> UPDATING DECK WORD WITH ID: {Id}", request.Request.Id);

        // VERIFY DECK WORD EXISTS
        var existingDeckWord = await deckWordRepository.GetByIdAsync(request.Request.Id);

        // FAST FAIL
        if (existingDeckWord is null)
        {
            logger.LogWarning("UpdateDeckWordCommandHandler -> DECK WORD NOT FOUND WITH ID: {Id}", request.Request.Id);
            return ServiceResult<int>.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }


        try
        {
            // UPDATE OTHER FIELDS
            existingDeckWord.FlashcardCategoryId = request.Request.FlashcardCategoryId;
            existingDeckWord.Question = request.Request.Question;
            existingDeckWord.Answer = request.Request.Answer;

            deckWordRepository.Update(existingDeckWord);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);

            logger.LogInformation("UpdateDeckWordCommandHandler -> SUCCESSFULLY UPDATED DECK WORD WITH ID: {Id}", existingDeckWord.Id);

            return ServiceResult<int>.Success(existingDeckWord.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateDeckWordCommandHandler -> ERROR UPDATING DECK WORD");
            return ServiceResult<int>.Fail("ERROR UPDATING DECK WORD", HttpStatusCode.InternalServerError);
        }
    }
}
