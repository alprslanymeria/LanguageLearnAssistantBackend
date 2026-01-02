using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Commands;

/// <summary>
/// COMMAND TO CREATE A NEW FLASHCARD CATEGORY.
/// </summary>
public record CreateFlashcardCategoryCommand(
    int FlashcardId,
    string Name,
    string UserId,
    int LanguageId) : ICommand<FlashcardCategoryDto>;
