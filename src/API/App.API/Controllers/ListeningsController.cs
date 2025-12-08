using App.Application.Features.Listenings.Dtos;
using App.Application.Features.Listenings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ListeningsController : ControllerBase
{
    private readonly IListeningService _listeningService;

    public ListeningsController(IListeningService listeningService)
    {
        _listeningService = listeningService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _listeningService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _listeningService.GetByUserIdAsync(userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _listeningService.GetByPracticeIdAsync(practiceId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _listeningService.GetByIdAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.OK => Ok(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListeningDto dto)
    {
        var result = await _listeningService.CreateAsync(dto);
        return result.IsSuccess ? Created(result.UrlAsCreated!, result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateListeningDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { ErrorMessage = new[] { "Id mismatch" } });
        }

        var result = await _listeningService.UpdateAsync(dto);
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
        var result = await _listeningService.DeleteAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.NoContent => NoContent(),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }
}
