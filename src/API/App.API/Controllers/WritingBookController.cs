using System.Security.Claims;
using App.API.Adapters;
using App.Application.Common;
using App.Application.Features.WritingBooks.Commands.CreateWritingBook;
using App.Application.Features.WritingBooks.Commands.DeleteWBookItemById;
using App.Application.Features.WritingBooks.Commands.UpdateWritingBook;
using App.Application.Features.WritingBooks.Dtos;
using App.Application.Features.WritingBooks.Queries.GetAllWBooksWithPaging;
using App.Application.Features.WritingBooks.Queries.GetWBookCreateItems;
using App.Application.Features.WritingBooks.Queries.GetWritingBookById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class WritingBookController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES A WRITING BOOK BY ID.
    /// /api/v1.0/WritingBook/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWritingBookItemById(int id) 
        => ActionResultInstance(await sender.Send(new GetWritingBookByIdQuery(id)));

    /// <summary>
    /// RETRIEVES ALL WRITING BOOKS WITH PAGING.
    /// /api/v1.0/WritingBook?Page=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllWBooksWithPaging([FromQuery] PagedRequest request)
        => ActionResultInstance(await sender.Send(new GetAllWBooksWithPagingQuery(UserId, request)));

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/WritingBook/create-items?language=en&practice=writing
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetWBookCreateItems([FromQuery] string language, string practice)
        => ActionResultInstance(await sender.Send(new GetWBookCreateItemsQuery(UserId, language, practice)));

    /// <summary>
    /// DELETES A WRITING BOOK BY ID.
    /// /api/v1.0/WritingBook/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWBookItemById(int id)
        => ActionResultInstance(await sender.Send(new DeleteWBookItemByIdCommand(id)));

    /// <summary>
    /// CREATES A NEW WRITING BOOK WITH FILE UPLOAD.
    /// /api/v1.0/WritingBook + FORM DATA
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> WritingBookAdd(

        [FromForm] CreateWritingBookRequest request,
        [FromForm] IFormFile imageFile,
        [FromForm] IFormFile sourceFile)
    {

        var requestWithExtra = request with
        {
            UserId = UserId,
            ImageFile = new FormFileUploadAdapter(imageFile),
            SourceFile = new FormFileUploadAdapter(sourceFile)
        };

        return ActionResultInstance(await sender.Send(new CreateWritingBookCommand(requestWithExtra)));
    }

    /// <summary>
    /// UPDATES AN EXISTING WRITING BOOK WITH OPTIONAL FILE UPLOAD.
    /// /api/v1.0/WritingBook + FORM DATA
    /// </summary>
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> WritingBookUpdate(

        [FromForm] UpdateWritingBookRequest request,
        [FromForm] IFormFile imageFile,
        [FromForm] IFormFile sourceFile)
    {

        var requestWithExtra = request with
        {
            UserId = UserId,
            ImageFile = new FormFileUploadAdapter(imageFile),
            SourceFile = new FormFileUploadAdapter(sourceFile)
        };      

        return ActionResultInstance(await sender.Send(new UpdateWritingBookCommand(requestWithExtra)));
    }
}
