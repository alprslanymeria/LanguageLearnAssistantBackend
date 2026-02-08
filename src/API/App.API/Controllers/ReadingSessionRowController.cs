using App.Application.Common;
using App.Application.Features.ReadingSessionRows.Commands.CreateRRows;
using App.Application.Features.ReadingSessionRows.Dtos;
using App.Application.Features.ReadingSessionRows.Queries.GetRRowsByIdWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReadingSessionRowController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL READING ROWS BY SESSION ID.
    /// /api/v1.0/ReadingSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetRRowsByIdWithPaging([FromQuery] PagedRequest request, string oldSessionId)
        => ActionResultInstance(await sender.Send(new GetRRowsByIdWithPagingQuery(request, oldSessionId)));


    /// <summary>
    /// SAVES READING SESSION ROWS.
    /// /api/v1.0/ReadingSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRRows([FromBody] SaveReadingRowsRequest request)
        => ActionResultInstance(await sender.Send(new CreateRRowsCommand(request)));
}
