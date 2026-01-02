using App.Application.Common;
using App.Application.Features.ListeningOldSessions;
using App.Application.Features.ListeningOldSessions.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Asp.Versioning;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ListeningOldSessionController(IListeningOldSessionService listeningOldSessionService) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL LISTENING OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/ListeningOldSession?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetListeningOldSessionsWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await listeningOldSessionService.GetListeningOldSessionsWithPagingAsync(UserId, request));


    /// <summary>
    /// SAVES A NEW LISTENING OLD SESSION.
    /// /api/v1.0/ListeningOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveListeningOldSession([FromBody] SaveListeningOldSessionRequest request)
        => ActionResultInstance(await listeningOldSessionService.SaveListeningOldSessionAsync(request));
}
