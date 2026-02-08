using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Queries.GetAllFCategories;

/// <summary>
/// QUERY FOR RETRIEVING ALL FLASHCARD CATEGORIES.
/// </summary>
public record GetAllFCategoriesQuery(string UserId) : IQuery<ServiceResult<FlashcardCategoryWithLanguageIds>>;
