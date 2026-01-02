using App.Application.Common;
using App.Application.Features.WritingSessionRows;
using App.Application.Features.WritingSessionRows.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class WritingSessionRowController(IWritingSessionRowService writingSessionRowService) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL WRITING ROWS BY SESSION ID.
    /// /api/v1.0/WritingSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetWritingRowsByIWithPaging([FromQuery] PagedRequest request, string oldSessionId) 
        => ActionResultInstance(await writingSessionRowService.GetWritingRowsByIWithPagingAsync(request, oldSessionId));


    /// <summary>
    /// SAVES WRITING SESSION ROWS.
    /// /api/v1.0/WritingSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveWritingRows([FromBody] SaveWritingRowsRequest request) 
        => ActionResultInstance(await writingSessionRowService.SaveWritingRowsAsync(request));
    
}
