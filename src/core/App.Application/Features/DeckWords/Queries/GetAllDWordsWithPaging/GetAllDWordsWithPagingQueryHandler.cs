using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;

namespace App.Application.Features.DeckWords.Queries.GetAllDWordsWithPaging;

/// <summary>
/// HANDLER FOR GET ALL DECK WORDS WITH PAGING QUERY.
/// </summary>
public class GetAllDWordsWithPagingQueryHandler(

    IDeckWordRepository deckWordRepository,
    IMapper mapper

    ) : IQueryHandler<GetAllDWordsWithPagingQuery, ServiceResult<PagedResult<DeckWordWithTotalCount>>>
{
    public async Task<ServiceResult<PagedResult<DeckWordWithTotalCount>>> Handle(

        GetAllDWordsWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        // GET ALL DECK WORDS WITH PAGING FROM REPOSITORY
        var (items, totalCount) = await deckWordRepository.GetAllDWordsWithPagingAsync(request.UserId, request.Request.Page, request.Request.PageSize);

        // MAP DECK WORDS TO DECK WORD DTOS
        var mappedDtos = mapper.Map<List<DeckWord>, List<DeckWordDto>>(items);

        var mappedResult = new DeckWordWithTotalCount(

            DeckWordDtos: mappedDtos,
            TotalCount: totalCount
            );

        var result = PagedResult<DeckWordWithTotalCount>.Create([mappedResult], request.Request, totalCount);

        return ServiceResult<PagedResult<DeckWordWithTotalCount>>.Success(result);
    }
}
