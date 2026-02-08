using App.Application.Common;
using App.Application.Features.FlashcardSessionRows.Commands.CreateFRows;
using App.Application.Features.FlashcardSessionRows.Dtos;
using App.Application.Features.FlashcardSessionRows.Queries.GetFRowsByIdWithPaging;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FlashcardSessionRowController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL FLASHCARD ROWS BY SESSION ID.
    /// /api/v1.0/FlashcardSessionRow/{oldSessionId}?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet("{oldSessionId}")]
    public async Task<IActionResult> GetFRowsByIdWithPaging([FromQuery] PagedRequest request, string oldSessionId)
        => ActionResultInstance(await sender.Send(new GetFRowsByIdWithPagingQuery(request, oldSessionId)));


    /// <summary>
    /// SAVES FLASHCARD SESSION ROWS.
    /// /api/v1.0/FlashcardSessionRow + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFRows([FromBody] SaveFlashcardRowsRequest request)
        => ActionResultInstance(await sender.Send(new CreateFRowsCommand(request)));

}
