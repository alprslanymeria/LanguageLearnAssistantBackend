using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks.Queries.GetWBookCreateItems;

/// <summary>
/// QUERY FOR RETRIEVING CREATE ITEMS FOR DROPDOWN SELECTIONS.
/// </summary>
public record GetWBookCreateItemsQuery(
    string UserId, 
    string Language, 
    string Practice
    ) : IQuery<ServiceResult<List<WritingBookDto>>>;
