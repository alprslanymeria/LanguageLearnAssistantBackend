using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks.Queries.GetAllWBooksWithPaging;

/// <summary>
/// QUERY FOR RETRIEVING ALL WRITING BOOKS WITH PAGING.
/// </summary>
public record GetAllWBooksWithPagingQuery(
    string UserId, 
    int Page, 
    int PageSize
    ) : IQuery<ServiceResult<PagedResult<WritingBookWithTotalCount>>>;
