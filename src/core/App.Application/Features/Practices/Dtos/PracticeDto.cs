namespace App.Application.Features.Practices.Dtos;

/// <summary>
/// RESPONSE DTO FOR PRACTICE ENTITY.
/// </summary>
public record PracticeDto
{
    public int LanguageId { get; init; }
    public string Name { get; init; } = null!;
}