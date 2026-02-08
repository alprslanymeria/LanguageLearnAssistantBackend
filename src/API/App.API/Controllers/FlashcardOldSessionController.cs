using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.FlashcardOldSessions.Commands.CreateFOS;
using App.Application.Features.FlashcardOldSessions.Dtos;
using App.Application.Features.FlashcardOldSessions.Queries.GetFOSWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FlashcardOldSessionController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL FLASHCARD OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/FlashcardOldSession?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFOSWithPaging([FromQuery] PagedRequest request, string language)
        => ActionResultInstance(await sender.Send(new GetFOSWithPagingQuery(UserId, language, request)));


    /// <summary>
    /// SAVES A NEW FLASHCARD OLD SESSION.
    /// /api/v1.0/FlashcardOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFOS([FromBody] SaveFlashcardOldSessionRequest request)
        => ActionResultInstance(await sender.Send(new CreateFOSCommand(request)));

}
