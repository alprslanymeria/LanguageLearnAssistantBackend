using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.DeckWords.Queries.GetDeckWordById;

/// <summary>
/// HANDLER FOR GET DECK WORD BY ID QUERY.
/// </summary>
public class GetDeckWordByIdQueryHandler(

    IDeckWordRepository deckWordRepository

    ) : IQueryHandler<GetDeckWordByIdQuery, ServiceResult<DeckWordWithLanguageId>>
{
    public async Task<ServiceResult<DeckWordWithLanguageId>> Handle(

        GetDeckWordByIdQuery request,
        CancellationToken cancellationToken)
    {

        // GET DECK WORD
        var deckWord = await deckWordRepository.GetDeckWordItemByIdAsync(request.Id)
            ?? throw new NotFoundException("DECK WORD NOT FOUND");

        return ServiceResult<DeckWordWithLanguageId>.Success(deckWord);
    }
}
