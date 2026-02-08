using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Commands.CreateFlashcardCategory;

/// <summary>
/// COMMAND FOR CREATING A NEW FLASHCARD CATEGORY.
/// </summary>
public record CreateFlashcardCategoryCommand(CreateFlashcardCategoryRequest Request) : ICommand<ServiceResult>;
