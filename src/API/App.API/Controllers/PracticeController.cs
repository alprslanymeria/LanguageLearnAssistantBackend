using App.Application.Features.Practices.Queries.GetPracticesByLanguage;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class PracticeController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES PRACTICES BY SPECIFIED LANGUAGE.
    /// /api/v1/Practice?language=en
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPracticesByLanguage([FromQuery] string language)
        => ActionResultInstance(await sender.Send(new GetPracticesByLanguageQuery(language)));
}
