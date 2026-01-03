using System.Security.Claims;
using App.API.Adapters;
using App.Application.Common;
using App.Application.Features.ReadingBooks.Commands.CreateReadingBook;
using App.Application.Features.ReadingBooks.Commands.DeleteRBookItemById;
using App.Application.Features.ReadingBooks.Commands.UpdateReadingBook;
using App.Application.Features.ReadingBooks.Queries.GetAllRBooksWithPaging;
using App.Application.Features.ReadingBooks.Queries.GetRBookCreateItems;
using App.Application.Features.ReadingBooks.Queries.GetReadingBookById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReadingBookController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES A READING BOOK BY ID.
    /// /api/v1.0/ReadingBook/{id}
    /// </summary>
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetReadingBookItemById(int id) 
        => ActionResultInstance(await sender.Send(new GetReadingBookByIdQuery(id)));

    /// <summary>
    /// RETRIEVES ALL READING BOOKS WITH PAGING.
    /// /api/v1.0/ReadingBook?Page=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRBooksWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await sender.Send(new GetAllRBooksWithPagingQuery(UserId, request.Page, request.PageSize)));

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/ReadingBook/create-items?language=en&practice=reading
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetRBookCreateItems([FromQuery] string language, string practice) 
        => ActionResultInstance(await sender.Send(new GetRBookCreateItemsQuery(UserId, language, practice)));

    /// <summary>
    /// DELETES A READING BOOK BY ID.
    /// /api/v1.0/ReadingBook/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRBookItemById(int id) 
        => ActionResultInstance(await sender.Send(new DeleteRBookItemByIdCommand(id)));

    /// <summary>
    /// CREATES A NEW READING BOOK WITH FILE UPLOAD.
    /// /api/v1.0/ReadingBook + FORM DATA
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReadingBookAdd(
        [FromForm] int readingId,
        [FromForm] string name,
        [FromForm] int languageId,
        [FromForm] IFormFile imageFile,
        [FromForm] IFormFile sourceFile)
    {
        var command = new CreateReadingBookCommand(
            readingId,
            name,
            new FormFileUploadAdapter(imageFile),
            new FormFileUploadAdapter(sourceFile),
            UserId,
            languageId);

        return ActionResultInstance(await sender.Send(command));
    }

    /// <summary>
    /// UPDATES AN EXISTING READING BOOK WITH OPTIONAL FILE UPLOAD.
    /// /api/v1.0/ReadingBook + FORM DATA
    /// </summary>
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReadingBookUpdate(
        [FromForm] int id,
        [FromForm] int readingId,
        [FromForm] string name,
        [FromForm] int languageId,
        [FromForm] IFormFile? imageFile,
        [FromForm] IFormFile? sourceFile)
    {
        var command = new UpdateReadingBookCommand(
            id,
            readingId,
            name,
            imageFile is not null ? new FormFileUploadAdapter(imageFile) : null,
            sourceFile is not null ? new FormFileUploadAdapter(sourceFile) : null,
            UserId,
            languageId);

        return ActionResultInstance(await sender.Send(command));
    }
}
