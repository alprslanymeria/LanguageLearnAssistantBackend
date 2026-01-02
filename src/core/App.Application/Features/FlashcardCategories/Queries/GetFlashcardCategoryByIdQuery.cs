using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries;

/// <summary>
/// QUERY TO GET FLASHCARD CATEGORY BY ID.
/// </summary>
public record GetFlashcardCategoryByIdQuery(int Id) : IQuery<FlashcardCategoryWithLanguageId>;
