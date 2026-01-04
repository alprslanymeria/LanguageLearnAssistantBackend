using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.FlashcardCategories.CacheKeys;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries.GetFlashcardCategoryById;

/// <summary>
/// QUERY FOR RETRIEVING A FLASHCARD CATEGORY BY ID.
/// </summary>
public record GetFlashcardCategoryByIdQuery(int Id) : IQuery<ServiceResult<FlashcardCategoryWithLanguageId>> , ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => FlashcardCategoryCacheKeys.ById(keyFactory, Id);
}
