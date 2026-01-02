using App.Application.Features.Languages;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class LanguageController(ILanguageService languageService) : BaseController
{
    /// <summary>
    /// RETRIEVES ALL LANGUAGES WITH PRACTICE COUNTS.
    /// /api/v1/Language
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetLanguages() => ActionResultInstance(await languageService.GetLanguagesAsync());
}
