using System.Security.Claims;
using App.Application.Common;
using App.Application.Features.ListeningOldSessions.Commands.CreateLOS;
using App.Application.Features.ListeningOldSessions.Dtos;
using App.Application.Features.ListeningOldSessions.Queries.GetLOSWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ListeningOldSessionController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES ALL LISTENING OLD SESSIONS FOR THE CURRENT USER.
    /// /api/v1.0/ListeningOldSession?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetListeningOldSessionsWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await sender.Send(new GetLOSWithPagingQuery(UserId, request)));


    /// <summary>
    /// SAVES A NEW LISTENING OLD SESSION.
    /// /api/v1.0/ListeningOldSession + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveListeningOldSession([FromBody] SaveListeningOldSessionRequest request)
        => ActionResultInstance(await sender.Send(new CreateLOSCommand(request)));
}
