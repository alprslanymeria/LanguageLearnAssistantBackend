using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.ReadingOldSessions;
using App.Application.Features.ReadingOldSessions.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReadingOldSessionController(IReadingOldSessionService readingOldSessionService) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL READING OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/ReadingOldSession?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetReadingOldSessionsWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await readingOldSessionService.GetReadingOldSessionsWithPagingAsync(UserId, request));


    /// <summary>
    /// SAVES A NEW READING OLD SESSION.
    /// /api/v1.0/ReadingOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveReadingOldSession([FromBody] SaveReadingOldSessionRequest request) 
        => ActionResultInstance(await readingOldSessionService.SaveReadingOldSessionAsync(request));
    
}
