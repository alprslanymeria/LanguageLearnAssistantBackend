using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.CacheKeys;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.DeckWords.Queries.GetDeckWordById;

/// <summary>
/// HANDLER FOR GET DECK WORD BY ID QUERY.
/// </summary>
public class GetDeckWordByIdQueryHandler(
    IDeckWordRepository deckWordRepository,
    IStaticCacheManager cacheManager,
    ICacheKeyFactory cacheKeyFactory,
    IMapper mapper,
    ILogger<GetDeckWordByIdQueryHandler> logger
    ) : IQueryHandler<GetDeckWordByIdQuery, ServiceResult<DeckWordWithLanguageId>>
{
    public async Task<ServiceResult<DeckWordWithLanguageId>> Handle(
        GetDeckWordByIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetDeckWordByIdQueryHandler -> FETCHING DECK WORD WITH ID: {Id}", request.Id);

        var cacheKey = DeckWordCacheKeys.ById(cacheKeyFactory, request.Id);

        var cachedResult = await cacheManager.GetAsync(cacheKey, async () =>
        {
            var deckWord = await deckWordRepository.GetDeckWordItemByIdAsync(request.Id);

            if (deckWord is null)
            {
                return null;
            }

            logger.LogInformation("GetDeckWordByIdQueryHandler -> SUCCESSFULLY FETCHED DECK WORD: {Question}", deckWord.Question);

            return mapper.Map<DeckWord, DeckWordWithLanguageId>(deckWord);
        });

        if (cachedResult is null)
        {
            logger.LogWarning("GetDeckWordByIdQueryHandler -> DECK WORD NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<DeckWordWithLanguageId>.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        return ServiceResult<DeckWordWithLanguageId>.Success(cachedResult);
    }
}
