using App.Application.Features.Languages.DTOs;
using App.Application.Features.Languages.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguagesController : ControllerBase
{
    private readonly ILanguageService _languageService;
    private readonly ILogger<LanguagesController> _logger;

    public LanguagesController(ILanguageService languageService, ILogger<LanguagesController> logger)
    {
        _languageService = languageService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all available languages
    /// </summary>
    /// <returns>List of languages</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<LanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLanguages()
    {
        var result = await _languageService.GetLanguagesAsync();

        if (result.IsFail)
        {
            return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Compares a language ID with the user's native language
    /// </summary>
    /// <param name="request">Request containing user ID and language ID to compare</param>
    /// <returns>True if the language matches the user's native language, false otherwise</returns>
    [HttpPost("compare")]
    [ProducesResponseType(typeof(CompareLanguageIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CompareLanguageId([FromBody] CompareLanguageIdRequest request)
    {
        var result = await _languageService.CompareLanguageIdAsync(request);

        if (result.IsFail)
        {
            return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
        }

        return Ok(result.Data);
    }
}
