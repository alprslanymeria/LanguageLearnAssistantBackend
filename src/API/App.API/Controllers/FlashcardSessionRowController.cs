using App.Application.Common;
using App.Application.Features.FlashcardSessionRows;
using App.Application.Features.FlashcardSessionRows.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FlashcardSessionRowController(IFlashcardSessionRowService flashcardSessionRowService) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL FLASHCARD ROWS BY SESSION ID.
    /// /api/v1.0/FlashcardSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetFlashcardRowsByIdWithPaging([FromQuery] PagedRequest request, string oldSessionId) 
        => ActionResultInstance(await flashcardSessionRowService.GetFlashcardRowsByIdWithPagingAsync(request, oldSessionId));


    /// <summary>
    /// SAVES FLASHCARD SESSION ROWS.
    /// /api/v1.0/FlashcardSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveFlashcardRows([FromBody] SaveFlashcardRowsRequest request) 
        => ActionResultInstance(await flashcardSessionRowService.SaveFlashcardRowsAsync(request));
    
}
