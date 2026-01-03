using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.FlashcardCategories.Commands.CreateFlashcardCategory;
using App.Application.Features.FlashcardCategories.Commands.DeleteFCategoryItemById;
using App.Application.Features.FlashcardCategories.Commands.UpdateFlashcardCategory;
using App.Application.Features.FlashcardCategories.Queries.GetAllFCategoriesWithPaging;
using App.Application.Features.FlashcardCategories.Queries.GetFCategoryCreateItems;
using App.Application.Features.FlashcardCategories.Queries.GetFlashcardCategoryById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FlashcardCategoryController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES A FLASHCARD CATEGORY BY ID.
    /// /api/v1.0/FlashcardCategory/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFlashcardCategoryItemById(int id) 
        => ActionResultInstance(await sender.Send(new GetFlashcardCategoryByIdQuery(id)));

    /// <summary>
    /// RETRIEVES ALL FLASHCARD CATEGORIES WITH PAGING.
    /// /api/v1.0/FlashcardCategory?Page=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllFCategoriesWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await sender.Send(new GetAllFCategoriesWithPagingQuery(UserId, request.Page, request.PageSize)));

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/FlashcardCategory/create-items?language=en&practice=grammar
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetFCategoryCreateItems([FromQuery] string language, string practice)
        => ActionResultInstance(await sender.Send(new GetFCategoryCreateItemsQuery(UserId, language, practice)));

    /// <summary>
    /// DELETES A FLASHCARD CATEGORY BY ID.
    /// /api/v1.0/FlashcardCategory/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFCategoryItemById(int id) 
        => ActionResultInstance(await sender.Send(new DeleteFCategoryItemByIdCommand(id)));

    /// <summary>
    /// CREATES A NEW FLASHCARD CATEGORY.
    /// /api/v1.0/FlashcardCategory + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> FlashcardCategoryAdd(
        [FromBody] CreateFlashcardCategoryRequest request)
    {
        var command = new CreateFlashcardCategoryCommand(
            request.FlashcardId,
            request.Name,
            UserId,
            request.LanguageId);

        return ActionResultInstance(await sender.Send(command));
    }

    /// <summary>
    /// UPDATES AN EXISTING FLASHCARD CATEGORY.
    /// /api/v1.0/FlashcardCategory + JSON BODY
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> FlashcardCategoryUpdate(
        [FromBody] UpdateFlashcardCategoryRequest request)
    {
        var command = new UpdateFlashcardCategoryCommand(
            request.Id,
            request.FlashcardId,
            request.Name,
            UserId,
            request.LanguageId);

        return ActionResultInstance(await sender.Send(command));
    }
}

/// <summary>
/// REQUEST DTO FOR CREATING A FLASHCARD CATEGORY FROM API.
/// </summary>
public record CreateFlashcardCategoryRequest(int FlashcardId, string Name, int LanguageId);

/// <summary>
/// REQUEST DTO FOR UPDATING A FLASHCARD CATEGORY FROM API.
/// </summary>
public record UpdateFlashcardCategoryRequest(int Id, int FlashcardId, string Name, int LanguageId);
