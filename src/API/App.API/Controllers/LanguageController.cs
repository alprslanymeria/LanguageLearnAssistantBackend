using App.Application.Features.Languages.Queries.GetLanguages;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class LanguageController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL LANGUAGES WITH PRACTICE COUNTS.
    /// /api/v1/Language
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetLanguages() 
        => ActionResultInstance(await sender.Send(new GetLanguagesQuery()));
}
