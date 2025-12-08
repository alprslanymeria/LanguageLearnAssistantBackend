using App.Application.Features.Flashcards.Dtos;
using App.Application.Features.Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class FlashcardsController : BaseController
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardsController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _flashcardService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _flashcardService.GetByUserIdAsync(userId);
        return HandleResult(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _flashcardService.GetByPracticeIdAsync(practiceId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _flashcardService.GetByIdAsync(id);
        return HandleResultWithStatus(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlashcardDto dto)
    {
        var result = await _flashcardService.CreateAsync(dto);
        return HandleCreateResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFlashcardDto dto)
    {
        var idMismatch = ValidateIdMatch(id, dto.Id);
        if (idMismatch != null) return idMismatch;

        var result = await _flashcardService.UpdateAsync(dto);
        return HandleResultWithStatus(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _flashcardService.DeleteAsync(id);
        return HandleDeleteResult(result);
    }
}
