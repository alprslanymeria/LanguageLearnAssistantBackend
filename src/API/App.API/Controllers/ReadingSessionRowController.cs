using App.Application.Common;
using App.Application.Features.ReadingSessionRows;
using App.Application.Features.ReadingSessionRows.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReadingSessionRowController(IReadingSessionRowService readingSessionRowService) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL READING ROWS BY SESSION ID.
    /// /api/v1.0/ReadingSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetReadingRowsByIdWithPaging([FromQuery] PagedRequest request, string oldSessionId) 
        => ActionResultInstance(await readingSessionRowService.GetReadingRowsByIdWithPagingAsync(request, oldSessionId));


    /// <summary>
    /// SAVES READING SESSION ROWS.
    /// /api/v1.0/ReadingSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SaveReadingRows([FromBody] SaveReadingRowsRequest request) 
        => ActionResultInstance(await readingSessionRowService.SaveReadingRowsAsync(request));
}