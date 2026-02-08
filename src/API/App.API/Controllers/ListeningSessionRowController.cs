using App.Application.Common;
using App.Application.Features.ListeningSessionRows.Commands.CreateLRows;
using App.Application.Features.ListeningSessionRows.Dtos;
using App.Application.Features.ListeningSessionRows.Queries.GetLRowsByIdWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ListeningSessionRowController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL LISTENING ROWS BY SESSION ID.
    /// /api/v1.0/ListeningSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetLRowsByIdWithPaging([FromQuery] PagedRequest request, string oldSessionId)
        => ActionResultInstance(await sender.Send(new GetLRowsByIdWithPagingQuery(request, oldSessionId)));


    /// <summary>
    /// SAVES LISTENING SESSION ROWS.
    /// /api/v1.0/ListeningSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateLRows([FromBody] SaveListeningRowsRequest request)
        => ActionResultInstance(await sender.Send(new CreateLRowsCommand(request)));
}
