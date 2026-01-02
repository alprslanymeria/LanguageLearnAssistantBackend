using App.Application.Common;
using App.Application.Features.ListeningSessionRows;
using App.Application.Features.ListeningSessionRows.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ListeningSessionRowController(IListeningSessionRowService listeningSessionRowService) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL LISTENING ROWS BY SESSION ID.
    /// /api/v1.0/ListeningSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetListeningRowsByIdWithPaging([FromQuery] PagedRequest request, string oldSessionId) 
        => ActionResultInstance(await listeningSessionRowService.GetListeningRowsByIdWithPagingAsync(request, oldSessionId));


    /// <summary>
    /// SAVES LISTENING SESSION ROWS.
    /// /api/v1.0/ListeningSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveListeningRows([FromBody] SaveListeningRowsRequest request) 
        => ActionResultInstance(await listeningSessionRowService.SaveListeningRowsAsync(request));
}
