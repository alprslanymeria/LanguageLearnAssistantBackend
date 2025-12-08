using App.Application.Features.Readings.Dtos;
using App.Application.Features.Readings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ReadingsController : BaseController
{
    private readonly IReadingService _readingService;

    public ReadingsController(IReadingService readingService)
    {
        _readingService = readingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _readingService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _readingService.GetByUserIdAsync(userId);
        return HandleResult(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _readingService.GetByPracticeIdAsync(practiceId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _readingService.GetByIdAsync(id);
        return HandleResultWithStatus(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReadingDto dto)
    {
        var result = await _readingService.CreateAsync(dto);
        return HandleCreateResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReadingDto dto)
    {
        var idMismatch = ValidateIdMatch(id, dto.Id);
        if (idMismatch != null) return idMismatch;

        var result = await _readingService.UpdateAsync(dto);
        return HandleResultWithStatus(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _readingService.DeleteAsync(id);
        return HandleDeleteResult(result);
    }
}
