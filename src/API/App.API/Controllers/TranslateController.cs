using System.Security.Claims;
using App.Application.Features.Translation.Dtos;
using App.Application.Features.Translation.Queries.TranslateText;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

/// <summary>
/// CONTROLLER FOR TEXT TRANSLATION OPERATIONS.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class TranslateController(ISender sender) : BaseController
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    /// <summary>
    /// TRANSLATES TEXT BASED ON PRACTICE TYPE AND USER PREFERENCES.
    /// FOR READING/LISTENING: TRANSLATES TO USER'S NATIVE LANGUAGE.
    /// FOR WRITING: TRANSLATES TO THE SPECIFIED TARGET LANGUAGE.
    /// /api/v1.0/Translate + JSON BODY
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> TranslateText([FromBody] TranslateTextRequest request, CancellationToken cancellationToken)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var result = await sender.Send(new TranslateTextQuery(request, UserId, accessToken!), cancellationToken);

        return ActionResultInstance(result);
    }
}
