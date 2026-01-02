using App.Application.Features.Practices.Queries;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class PracticeController(ISender sender) : BaseController
{
    /// <summary>
    /// RETRIEVES PRACTICES BY SPECIFIED LANGUAGE.
    /// /api/v1/Practice?language=en
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPracticesByLanguage([FromQuery] string language)
    {
        var query = new GetPracticesByLanguageQuery(language);
        var result = await sender.Send(query);
        return ActionResultInstance(result);
    }
}
