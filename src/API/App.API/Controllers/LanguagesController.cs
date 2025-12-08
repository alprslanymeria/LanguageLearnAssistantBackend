using App.Application.Features.Languages.Dtos;
using App.Application.Features.Languages.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class LanguagesController : BaseController
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
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _languageService.GetByIdAsync(id);
        return HandleResultWithStatus(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLanguageDto dto)
    {
        var result = await _languageService.CreateAsync(dto);
        return HandleCreateResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLanguageDto dto)
    {
        var idMismatch = ValidateIdMatch(id, dto.Id);
        if (idMismatch != null) return idMismatch;

        var result = await _languageService.UpdateAsync(dto);
        return HandleResultWithStatus(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _languageService.DeleteAsync(id);
        return HandleDeleteResult(result);
    }
}
