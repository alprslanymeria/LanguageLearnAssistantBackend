using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.FlashcardCategories;
using App.Application.Features.FlashcardCategories.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FlashcardCategoryController(IFlashcardCategoryService flashcardCategoryService) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES A FLASHCARD CATEGORY BY ID.
    /// /api/v1.0/FlashcardCategory/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFlashcardCategoryItemById(int id) 
        => ActionResultInstance(await flashcardCategoryService.GetFlashcardCategoryItemByIdAsync(id));

    /// <summary>
    /// RETRIEVES ALL FLASHCARD CATEGORIES WITH PAGING.
    /// /api/v1.0/FlashcardCategory?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllFCategoriesWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await flashcardCategoryService.GetAllFCategoriesWithPagingAsync(UserId, request));

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/FlashcardCategory/create-items?language=en&practice=grammar
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetFCategoryCreateItems([FromQuery] string language, string practice)
        => ActionResultInstance(await flashcardCategoryService.GetFCategoryCreateItemsAsync(UserId, language, practice));

    /// <summary>
    /// DELETES A FLASHCARD CATEGORY BY ID.
    /// /api/v1.0/FlashcardCategory/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFCategoryItemById(int id) 
        => ActionResultInstance(await flashcardCategoryService.DeleteFCategoryItemByIdAsync(id));


    /// <summary>
    /// CREATES A NEW FLASHCARD CATEGORY.
    /// /api/v1.0/FlashcardCategory + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> FlashcardCategoryAdd([FromBody] CreateFlashcardCategoryRequest request)
    {
        // SET USER ID FROM TOKEN
        var requestWithUserId = request with { UserId = UserId };
        return ActionResultInstance(await flashcardCategoryService.FlashcardCategoryAddAsync(requestWithUserId));
    }

    /// <summary>
    /// UPDATES AN EXISTING FLASHCARD CATEGORY.
    /// /api/v1.0/FlashcardCategory + JSON BODY
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> FlashcardCategoryUpdate([FromBody] UpdateFlashcardCategoryRequest request)
    {
        // SET USER ID FROM TOKEN
        var requestWithUserId = request with { UserId = UserId };
        return ActionResultInstance(await flashcardCategoryService.FlashcardCategoryUpdateAsync(requestWithUserId));
    }
}
