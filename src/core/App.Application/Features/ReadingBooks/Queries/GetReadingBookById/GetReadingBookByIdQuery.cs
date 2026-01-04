using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Queries.GetReadingBookById;

/// <summary>
/// QUERY FOR RETRIEVING A READING BOOK BY ID.
/// </summary>
public record GetReadingBookByIdQuery(int Id) : IQuery<ServiceResult<ReadingBookWithLanguageId>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => ReadingBookCacheKeys.ById(keyFactory, Id);
}
