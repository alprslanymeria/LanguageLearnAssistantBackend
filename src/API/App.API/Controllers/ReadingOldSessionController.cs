using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.ReadingOldSessions.Commands.SaveReadingOldSession;
using App.Application.Features.ReadingOldSessions.Queries.GetReadingOldSessionsWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReadingOldSessionController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL READING OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/ReadingOldSession?Page=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetReadingOldSessionsWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await sender.Send(new GetReadingOldSessionsWithPagingQuery(UserId, request.Page, request.PageSize)));

    /// <summary>
    /// SAVES A NEW READING OLD SESSION.
    /// /api/v1.0/ReadingOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveReadingOldSession([FromBody] SaveReadingOldSessionApiRequest request)
    {
        var command = new SaveReadingOldSessionCommand(
            request.Id,
            request.ReadingId,
            request.ReadingBookId,
            request.Rate);

        return ActionResultInstance(await sender.Send(command));
    }
}

/// <summary>
/// REQUEST DTO FOR SAVING A READING OLD SESSION FROM API.
/// </summary>
public record SaveReadingOldSessionApiRequest(string Id, int ReadingId, int ReadingBookId, decimal Rate);
