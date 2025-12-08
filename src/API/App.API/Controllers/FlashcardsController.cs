using App.Application.Features.Flashcards.Dtos;
using App.Application.Features.Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FlashcardsController : ControllerBase
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
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _flashcardService.GetByUserIdAsync(userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _flashcardService.GetByPracticeIdAsync(practiceId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _flashcardService.GetByIdAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.OK => Ok(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlashcardDto dto)
    {
        var result = await _flashcardService.CreateAsync(dto);
        return result.IsSuccess ? Created(result.UrlAsCreated!, result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFlashcardDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { ErrorMessage = new[] { "Id mismatch" } });
        }

        var result = await _flashcardService.UpdateAsync(dto);
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
        var result = await _flashcardService.DeleteAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.NoContent => NoContent(),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }
}
