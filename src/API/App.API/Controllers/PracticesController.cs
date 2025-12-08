using App.Application.Features.Practices.Dtos;
using App.Application.Features.Practices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PracticesController : BaseController
{
    private readonly IPracticeService _practiceService;

    public PracticesController(IPracticeService practiceService)
    {
        _practiceService = practiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _practiceService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("language/{languageId:int}")]
    public async Task<IActionResult> GetByLanguageId(int languageId)
    {
        var result = await _practiceService.GetByLanguageIdAsync(languageId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _practiceService.GetByIdAsync(id);
        return HandleResultWithStatus(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePracticeDto dto)
    {
        var result = await _practiceService.CreateAsync(dto);
        return HandleCreateResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePracticeDto dto)
    {
        var idMismatch = ValidateIdMatch(id, dto.Id);
        if (idMismatch != null) return idMismatch;

        var result = await _practiceService.UpdateAsync(dto);
        return HandleResultWithStatus(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _practiceService.DeleteAsync(id);
        return HandleDeleteResult(result);
    }
}
