using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.DeckWords.Commands.CreateDeckWord;

/// <summary>
/// HANDLER FOR CREATE DECK WORD COMMAND.
/// </summary>
public class CreateDeckWordCommandHandler(

    IDeckWordRepository deckWordRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager

    ) : ICommandHandler<CreateDeckWordCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(

        CreateDeckWordCommand request,
        CancellationToken cancellationToken)
    {

        // GET CATEGORY
        var category = await flashcardCategoryRepository.GetByIdAsync(request.Request.CategoryId)
            ?? throw new NotFoundException("FLASHCARD CATEGORY NOT FOUND");

        // CREATE DECK WORD
        var deckWord = new DeckWord
        {
            FlashcardCategoryId = category.Id,
            Question = request.Request.Word,
            Answer = request.Request.Answer,
            FlashcardCategory = category
        };

        // ADD DECK WORD TO DB AND COMMIT
        await deckWordRepository.AddAsync(deckWord);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(DeckWordCacheKeys.Prefix);

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
