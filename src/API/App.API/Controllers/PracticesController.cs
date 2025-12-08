using App.Application.Features.Practices.Dtos;
using App.Application.Features.Practices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PracticesController : ControllerBase
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
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("language/{languageId:int}")]
    public async Task<IActionResult> GetByLanguageId(int languageId)
    {
        var result = await _practiceService.GetByLanguageIdAsync(languageId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _practiceService.GetByIdAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.OK => Ok(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePracticeDto dto)
    {
        var result = await _practiceService.CreateAsync(dto);
        return result.IsSuccess ? Created(result.UrlAsCreated!, result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePracticeDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { ErrorMessage = new[] { "Id mismatch" } });
        }

        var result = await _practiceService.UpdateAsync(dto);
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
        var result = await _practiceService.DeleteAsync(id);
        return result.Status switch
        {
            System.Net.HttpStatusCode.NoContent => NoContent(),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }
}
