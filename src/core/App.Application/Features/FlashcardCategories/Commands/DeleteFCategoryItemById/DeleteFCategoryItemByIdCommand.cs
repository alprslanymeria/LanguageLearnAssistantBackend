using App.Application.Common;
using App.Application.Common.CQRS;

namespace App.Application.Features.FlashcardCategories.Commands.DeleteFCategoryItemById;

/// <summary>
/// COMMAND FOR DELETING A FLASHCARD CATEGORY BY ID.
/// </summary>
public record DeleteFCategoryItemByIdCommand(int Id) : ICommand<ServiceResult>;
