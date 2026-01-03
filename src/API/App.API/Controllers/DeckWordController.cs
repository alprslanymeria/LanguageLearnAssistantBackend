using App.Application.Common;
using App.Application.Features.DeckWords.Commands.CreateDeckWord;
using App.Application.Features.DeckWords.Commands.DeleteDWordItemById;
using App.Application.Features.DeckWords.Commands.UpdateDeckWord;
using App.Application.Features.DeckWords.Queries.GetAllDWordsWithPaging;
using App.Application.Features.DeckWords.Queries.GetDeckWordById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class DeckWordController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES A DECK WORD BY ID.
    /// /api/v1.0/DeckWord/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDeckWordItemById(int id) 
        => ActionResultInstance(await sender.Send(new GetDeckWordByIdQuery(id)));

    /// <summary>
    /// RETRIEVES ALL DECK WORDS WITH PAGING FOR A SPECIFIC CATEGORY.
    /// /api/v1.0/DeckWord/category/{categoryId}?Page=1&PageSize=10
    /// </summary>
    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetAllDWordsWithPaging(int categoryId, [FromQuery] PagedRequest request) 
        => ActionResultInstance(await sender.Send(new GetAllDWordsWithPagingQuery(categoryId, request.Page, request.PageSize)));

    /// <summary>
    /// DELETES A DECK WORD BY ID.
    /// /api/v1.0/DeckWord/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDWordItemById(int id) 
        => ActionResultInstance(await sender.Send(new DeleteDWordItemByIdCommand(id)));

    /// <summary>
    /// CREATES A NEW DECK WORD.
    /// /api/v1.0/DeckWord + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> DeckWordAdd([FromBody] CreateDeckWordApiRequest request)
    {
        var command = new CreateDeckWordCommand(
            request.FlashcardCategoryId,
            request.Question,
            request.Answer);

        return ActionResultInstance(await sender.Send(command));
    }

    /// <summary>
    /// UPDATES AN EXISTING DECK WORD.
    /// /api/v1.0/DeckWord + JSON BODY
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> DeckWordUpdate([FromBody] UpdateDeckWordApiRequest request)
    {
        var command = new UpdateDeckWordCommand(
            request.Id,
            request.FlashcardCategoryId,
            request.Question,
            request.Answer);

        return ActionResultInstance(await sender.Send(command));
    }
}

/// <summary>
/// REQUEST DTO FOR CREATING A DECK WORD FROM API.
/// </summary>
public record CreateDeckWordApiRequest(int FlashcardCategoryId, string Question, string Answer);

/// <summary>
/// REQUEST DTO FOR UPDATING A DECK WORD FROM API.
/// </summary>
public record UpdateDeckWordApiRequest(int Id, int FlashcardCategoryId, string Question, string Answer);
