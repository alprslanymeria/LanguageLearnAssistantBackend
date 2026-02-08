using System.Security.Claims;
using App.Application.Features.ListeningCategories.Queries.GetLCategoryCreateItems;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ListeningCategoryController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// /api/v1.0/ListeningCategory/create-items?language=en&practice=grammar
    /// </summary>
    [HttpGet("create-items")]
    public async Task<IActionResult> GetLCategoryCreateItems([FromQuery] string language, string practice)
        => ActionResultInstance(await sender.Send(new GetLCategoryCreateItemsQuery(UserId, language, practice)));
}
