using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.WritingOldSessions;
using App.Application.Features.WritingOldSessions.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class WritingOldSessionController(IWritingOldSessionService writingOldSessionService) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL WRITING OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/WritingOldSession?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetWritingOldSessionsWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await writingOldSessionService.GetWritingOldSessionsWithPagingAsync(UserId, request));


    /// <summary>
    /// SAVES A NEW WRITING OLD SESSION.
    /// /api/v1.0/WritingOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveWritingOldSession([FromBody] SaveWritingOldSessionRequest request) 
        => ActionResultInstance(await writingOldSessionService.SaveWritingOldSessionAsync(request));
    
}
