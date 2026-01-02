using System.Security.Claims;
using App.API.Adapters;
using App.Application.Common;
using App.Application.Features.ReadingBooks;
using App.Application.Features.ReadingBooks.Dtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReadingBookController(IReadingBookService readingBookService) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES A READING BOOK BY ID.
    /// /api/v1.0/ReadingBook/{id}
    /// </summary>
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetReadingBookItemById(int id) 
        => ActionResultInstance(await readingBookService.GetReadingBookItemByIdAsync(id));


    /// <summary>
    /// RETRIEVES ALL READING BOOKS WITH PAGING.
    /// /api/v1.0/ReadingBook?PageNumber=1&PageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRBooksWithPaging([FromQuery] PagedRequest request) 
        => ActionResultInstance(await readingBookService.GetAllRBooksWithPagingAsync(UserId, request));

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/ReadingBook/create-items?language=en&practice=reading
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetRBookCreateItems([FromQuery] string language, string practice) 
        => ActionResultInstance(await readingBookService.GetRBookCreateItemsAsync(UserId, language, practice));


    /// <summary>
    /// DELETES A READING BOOK BY ID.
    /// /api/v1.0/ReadingBook/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRBookItemById(int id) 
        => ActionResultInstance(await readingBookService.DeleteRBookItemByIdAsync(id));


    /// <summary>
    /// CREATES A NEW READING BOOK WITH FILE UPLOAD.
    /// /api/v1.0/ReadingBook + FORM DATA
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReadingBookAdd(

        [FromForm] CreateReadingBookRequest request,
        [FromForm] IFormFile imageFile,
        [FromForm] IFormFile sourceFile)
    {
        var requestWithExtra = request with
        {
            UserId = UserId,
            ImageFile = new FormFileUploadAdapter(imageFile),
            SourceFile = new FormFileUploadAdapter(sourceFile)
        };
        
        return ActionResultInstance(await readingBookService.ReadingBookAddAsync(requestWithExtra));
    }

    /// <summary>
    /// UPDATES AN EXISTING READING BOOK WITH OPTIONAL FILE UPLOAD.
    /// /api/v1.0/ReadingBook + FORM DATA
    /// </summary>
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ReadingBookUpdate(

        [FromForm] UpdateReadingBookRequest request,
        [FromForm] IFormFile imageFile,
        [FromForm] IFormFile sourceFile)
    {
        var requestWithExtra = request with
        {
            UserId = UserId,
            ImageFile = new FormFileUploadAdapter(imageFile),
            SourceFile = new FormFileUploadAdapter(sourceFile)
        };
        
        return ActionResultInstance(await readingBookService.ReadingBookUpdateAsync(requestWithExtra));
    }
}
