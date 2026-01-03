using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Queries.GetRBookCreateItems;

/// <summary>
/// QUERY FOR RETRIEVING CREATE ITEMS FOR DROPDOWN SELECTIONS.
/// </summary>
public record GetRBookCreateItemsQuery(
    string UserId, 
    string Language, 
    string Practice
    ) : IQuery<ServiceResult<List<ReadingBookDto>>>;
