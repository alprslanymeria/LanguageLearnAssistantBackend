using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Commands.UpdateFlashcardCategory;

/// <summary>
/// COMMAND FOR UPDATING AN EXISTING FLASHCARD CATEGORY.
/// </summary>
public record UpdateFlashcardCategoryCommand(
    int Id,
    int FlashcardId,
    string Name,
    string UserId,
    int LanguageId
    ) : ICommand<ServiceResult<FlashcardCategoryDto>>;
