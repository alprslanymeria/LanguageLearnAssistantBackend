using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.FlashcardOldSessions;
using App.Application.Features.FlashcardOldSessions.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FlashcardOldSessionController(IFlashcardOldSessionService flashcardOldSessionService) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL FLASHCARD OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/FlashcardOldSession?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFlashcardOldSessionsWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await flashcardOldSessionService.GetFlashcardOldSessionsWithPagingAsync(UserId, request));


    /// <summary>
    /// SAVES A NEW FLASHCARD OLD SESSION.
    /// /api/v1.0/FlashcardOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveFlashcardOldSession([FromBody] SaveFlashcardOldSessionRequest request) 
        => ActionResultInstance(await flashcardOldSessionService.SaveFlashcardOldSessionAsync(request));
    
}
