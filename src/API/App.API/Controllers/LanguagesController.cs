using App.Application.Features.Languages.Dtos;
using App.Application.Features.Languages.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LanguagesController : ControllerBase
{
    private readonly ILanguageService _languageService;

    public LanguagesController(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _languageService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _languageService.GetByIdAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.OK => Ok(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLanguageDto dto)
    {
        var result = await _languageService.CreateAsync(dto);
        return result.IsSuccess ? Created(result.UrlAsCreated!, result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLanguageDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { ErrorMessage = new[] { "Id mismatch" } });
        }

        var result = await _languageService.UpdateAsync(dto);
        return result.Status switch
        {
            System.Net.HttpStatusCode.OK => Ok(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _languageService.DeleteAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.NoContent => NoContent(),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }
}
