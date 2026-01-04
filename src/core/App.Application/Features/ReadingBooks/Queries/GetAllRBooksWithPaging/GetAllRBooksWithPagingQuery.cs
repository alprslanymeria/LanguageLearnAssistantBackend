using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Queries.GetAllRBooksWithPaging;

/// <summary>
/// QUERY FOR RETRIEVING ALL READING BOOKS WITH PAGING.
/// </summary>
public record GetAllRBooksWithPagingQuery(string UserId, PagedRequest Request) : IQuery<ServiceResult<PagedResult<ReadingBookWithTotalCount>>>;
