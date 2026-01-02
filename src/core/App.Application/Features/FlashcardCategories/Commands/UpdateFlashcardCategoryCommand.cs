using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Commands;

/// <summary>
/// COMMAND TO UPDATE AN EXISTING FLASHCARD CATEGORY.
/// </summary>
public record UpdateFlashcardCategoryCommand(
    int Id,
    int FlashcardId,
    string Name,
    string UserId,
    int LanguageId) : ICommand<FlashcardCategoryDto>;
