using App.Application.Features.Writings.Dtos;
using App.Application.Features.Writings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class WritingsController : BaseController
{
    private readonly IWritingService _writingService;

    public WritingsController(IWritingService writingService)
    {
        _writingService = writingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _writingService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _writingService.GetByUserIdAsync(userId);
        return HandleResult(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _writingService.GetByPracticeIdAsync(practiceId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _writingService.GetByIdAsync(id);
        return HandleResultWithStatus(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWritingDto dto)
    {
        var result = await _writingService.CreateAsync(dto);
        return HandleCreateResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWritingDto dto)
    {
        var idMismatch = ValidateIdMatch(id, dto.Id);
        if (idMismatch != null) return idMismatch;

        var result = await _writingService.UpdateAsync(dto);
        return HandleResultWithStatus(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _writingService.DeleteAsync(id);
        return HandleDeleteResult(result);
    }
}
