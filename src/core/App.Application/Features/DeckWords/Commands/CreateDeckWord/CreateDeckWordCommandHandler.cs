using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.DeckWords.Commands.CreateDeckWord;

/// <summary>
/// HANDLER FOR CREATE DECK WORD COMMAND.
/// </summary>
public class CreateDeckWordCommandHandler(

    IDeckWordRepository deckWordRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateDeckWordCommandHandler> logger,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<CreateDeckWordCommand, ServiceResult<int>>
{
    public async Task<ServiceResult<int>> Handle(

        CreateDeckWordCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateDeckWordCommandHandler -> CREATING NEW DECK WORD FOR CATEGORY ID: {CategoryId}", request.Request.FlashcardCategoryId);

        // IN HERE WE DON'T NEED TO CHECK PREVIOUS ENTRIES BECAUSE WE ARE MAKING A DEPENDENT SELECTION.
        // A MEAN USER CANNOT CREATE A WORD IF THERE IS NO CATEGORY.

        var category = await flashcardCategoryRepository.GetByIdAsync(request.Request.FlashcardCategoryId);

        try
        {
            // CREATE DECK WORD
            var deckWord = new DeckWord
            {
                FlashcardCategoryId = category.Id,
                Question = request.Request.Question,
                Answer = request.Request.Answer,
                FlashcardCategory = category!
            };

            await deckWordRepository.CreateAsync(deckWord);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);

            logger.LogInformation("CreateDeckWordCommandHandler -> SUCCESSFULLY CREATED DECK WORD WITH ID: {Id}, QUESTION: {Question}", deckWord.Id, deckWord.Question);

            return ServiceResult<int>.SuccessAsCreated(deckWord.Id, $"/api/DeckWord/{deckWord.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateDeckWordCommandHandler -> ERROR CREATING DECK WORD");
            return ServiceResult<int>.Fail("ERROR CREATING DECK WORD", HttpStatusCode.InternalServerError);
        }
    }
}
