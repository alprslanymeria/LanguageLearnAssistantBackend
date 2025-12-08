namespace App.Application.Features.Languages.DTOs;

public record LanguageDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? ImageUrl { get; init; }
}
