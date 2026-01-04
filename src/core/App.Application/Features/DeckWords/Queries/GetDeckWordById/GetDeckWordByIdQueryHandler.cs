using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.DeckWords.Queries.GetDeckWordById;

/// <summary>
/// HANDLER FOR GET DECK WORD BY ID QUERY.
/// </summary>
public class GetDeckWordByIdQueryHandler(

    IDeckWordRepository deckWordRepository,
    IMapper mapper,
    ILogger<GetDeckWordByIdQueryHandler> logger

    ) : IQueryHandler<GetDeckWordByIdQuery, ServiceResult<DeckWordWithLanguageId>>
{
    public async Task<ServiceResult<DeckWordWithLanguageId>> Handle(

        GetDeckWordByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var id = request.Id;

        logger.LogInformation("GetDeckWordByIdQueryHandler --> FETCHING DECK WORD WITH ID: {Id}", id);

        var deckWord = await deckWordRepository.GetDeckWordItemByIdAsync(id);

        if (deckWord is null)
        {
            logger.LogWarning("GetDeckWordByIdQueryHandler --> DECK WORD NOT FOUND WITH ID: {Id}", id);
            return ServiceResult<DeckWordWithLanguageId>.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("GetDeckWordByIdQueryHandler --> SUCCESSFULLY FETCHED DECK WORD: {WordName}", deckWord.Question);

        var result = mapper.Map<DeckWord, DeckWordWithLanguageId>(deckWord);
        return ServiceResult<DeckWordWithLanguageId>.Success(result);
    }
}
