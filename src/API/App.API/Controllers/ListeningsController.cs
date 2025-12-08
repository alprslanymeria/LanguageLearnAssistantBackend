using App.Application.Features.Listenings.Dtos;
using App.Application.Features.Listenings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ListeningsController : BaseController
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
        return HandleResult(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var result = await _listeningService.GetByUserIdAsync(userId);
        return HandleResult(result);
    }

    [HttpGet("practice/{practiceId:int}")]
    public async Task<IActionResult> GetByPracticeId(int practiceId)
    {
        var result = await _listeningService.GetByPracticeIdAsync(practiceId);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _listeningService.GetByIdAsync(id);
        return HandleResultWithStatus(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListeningDto dto)
    {
        var result = await _listeningService.CreateAsync(dto);
        return HandleCreateResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateListeningDto dto)
    {
        var idMismatch = ValidateIdMatch(id, dto.Id);
        if (idMismatch != null) return idMismatch;

        var result = await _listeningService.UpdateAsync(dto);
        return HandleResultWithStatus(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _listeningService.DeleteAsync(id);
        return HandleDeleteResult(result);
    }
}
