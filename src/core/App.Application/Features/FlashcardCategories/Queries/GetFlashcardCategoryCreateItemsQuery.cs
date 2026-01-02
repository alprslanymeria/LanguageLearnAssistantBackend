using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// QUERY TO GET FLASHCARD CATEGORY CREATE ITEMS.
/// </summary>
public record GetFlashcardCategoryCreateItemsQuery(string UserId, string Language, string Practice) 
    : IQuery<List<FlashcardCategoryDto>>;
