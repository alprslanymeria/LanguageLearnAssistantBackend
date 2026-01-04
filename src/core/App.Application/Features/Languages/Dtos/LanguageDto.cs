namespace App.Application.Features.Languages.Dtos;

/// <summary>
/// RESPONSE DTO FOR LANGUAGE ENTITY.
/// </summary>
public record LanguageDto
{
    public string Name { get; init; } = null!;
    public string? ImageUrl { get; init; }
}
