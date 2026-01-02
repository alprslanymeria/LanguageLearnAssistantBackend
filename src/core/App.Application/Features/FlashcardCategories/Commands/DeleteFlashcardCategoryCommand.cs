using App.Application.Common.CQRS;

namespace App.Application.Features.FlashcardCategories.Commands;

/// <summary>
/// COMMAND TO DELETE A FLASHCARD CATEGORY BY ID.
/// </summary>
public record DeleteFlashcardCategoryCommand(int Id) : ICommand;
