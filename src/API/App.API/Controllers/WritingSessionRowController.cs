using App.Application.Common;
using App.Application.Features.WritingSessionRows.Commands.CreateWRows;
using App.Application.Features.WritingSessionRows.Dtos;
using App.Application.Features.WritingSessionRows.Queries.GetWRowsByIdWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class WritingSessionRowController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL WRITING ROWS BY SESSION ID.
    /// /api/v1.0/WritingSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetWritingRowsByIWithPaging([FromQuery] PagedRequest request, string oldSessionId) 
        => ActionResultInstance(await sender.Send(new GetWRowsByIdWithPagingQuery(request, oldSessionId)));


    /// <summary>
    /// SAVES WRITING SESSION ROWS.
    /// /api/v1.0/WritingSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveWritingRows([FromBody] SaveWritingRowsRequest request) 
        => ActionResultInstance(await sender.Send(new CreateWRowsCommand(request)));

}
