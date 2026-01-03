using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords.Queries.GetAllDWordsWithPaging;

/// <summary>
/// QUERY FOR RETRIEVING ALL DECK WORDS WITH PAGING FOR A SPECIFIC CATEGORY.
/// </summary>
public record GetAllDWordsWithPagingQuery(
    int CategoryId, 
    int Page, 
    int PageSize
    ) : IQuery<ServiceResult<PagedResult<DeckWordWithTotalCount>>>;
