using App.Application.Features.Practices;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class PracticeController(IPracticeService practiceService) : BaseController
{
    /// <summary>
    /// RETRIEVES PRACTICES BY SPECIFIED LANGUAGE.
    /// /api/v1/Practice?language=en
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPracticesByLanguage([FromQuery] string language)
        => ActionResultInstance(await practiceService.GetPracticesByLanguageAsync(language));

}
