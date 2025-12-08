using App.Application.Features.Writings.Dtos;
using App.Application.Features.Writings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WritingsController : ControllerBase
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
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _writingService.GetByUserIdAsync(userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _writingService.GetByPracticeIdAsync(practiceId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _writingService.GetByIdAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.OK => Ok(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWritingDto dto)
    {
        var result = await _writingService.CreateAsync(dto);
        return result.IsSuccess ? Created(result.UrlAsCreated!, result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWritingDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { ErrorMessage = new[] { "Id mismatch" } });
        }

        var result = await _writingService.UpdateAsync(dto);
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
        var result = await _writingService.DeleteAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.NoContent => NoContent(),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }
}
