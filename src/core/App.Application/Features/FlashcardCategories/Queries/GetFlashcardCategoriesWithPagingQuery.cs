using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// QUERY TO GET ALL FLASHCARD CATEGORIES WITH PAGING.
/// </summary>
public record GetFlashcardCategoriesWithPagingQuery(string UserId, PagedRequest PagedRequest) 
    : IQuery<PagedResult<FlashcardCategoryWithTotalCount>>;
