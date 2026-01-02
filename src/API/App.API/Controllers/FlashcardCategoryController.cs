using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.FlashcardCategories.Commands;
using App.Application.Features.FlashcardCategories.Queries;
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
    {
        var query = new GetFlashcardCategoryByIdQuery(id);
        var result = await sender.Send(query);
        return ActionResultInstance(result);
    }

    /// <summary>
    /// RETRIEVES ALL FLASHCARD CATEGORIES WITH PAGING.
    /// /api/v1.0/FlashcardCategory?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllFCategoriesWithPaging([FromQuery] PagedRequest request)
    {
        var query = new GetFlashcardCategoriesWithPagingQuery(UserId, request);
        var result = await sender.Send(query);
        return ActionResultInstance(result);
    }

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/FlashcardCategory/create-items?language=en&practice=grammar
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetFCategoryCreateItems([FromQuery] string language, string practice)
    {
        var query = new GetFlashcardCategoryCreateItemsQuery(UserId, language, practice);
        var result = await sender.Send(query);
        return ActionResultInstance(result);
    }

    /// <summary>
    /// DELETES A FLASHCARD CATEGORY BY ID.
    /// /api/v1.0/FlashcardCategory/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFCategoryItemById(int id)
    {
        var command = new DeleteFlashcardCategoryCommand(id);
        var result = await sender.Send(command);
        return ActionResultInstance(result);
    }


    /// <summary>
    /// CREATES A NEW FLASHCARD CATEGORY.
    /// /api/v1.0/FlashcardCategory + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> FlashcardCategoryAdd([FromBody] CreateFlashcardCategoryCommand command)
    {
        // SET USER ID FROM TOKEN
        var commandWithUserId = command with { UserId = UserId };
        var result = await sender.Send(commandWithUserId);
        return ActionResultInstance(result);
    }

    /// <summary>
    /// UPDATES AN EXISTING FLASHCARD CATEGORY.
    /// /api/v1.0/FlashcardCategory + JSON BODY
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> FlashcardCategoryUpdate([FromBody] UpdateFlashcardCategoryCommand command)
    {
        // SET USER ID FROM TOKEN
        var commandWithUserId = command with { UserId = UserId };
        var result = await sender.Send(commandWithUserId);
        return ActionResultInstance(result);
    }
}
