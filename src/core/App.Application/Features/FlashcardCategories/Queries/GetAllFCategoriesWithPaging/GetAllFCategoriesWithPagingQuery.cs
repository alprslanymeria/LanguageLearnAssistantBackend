using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries.GetAllFCategoriesWithPaging;

/// <summary>
/// QUERY FOR RETRIEVING ALL FLASHCARD CATEGORIES WITH PAGING.
/// </summary>
public record GetAllFCategoriesWithPagingQuery(
    string UserId, 
    int Page, 
    int PageSize
    ) : IQuery<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>>;
