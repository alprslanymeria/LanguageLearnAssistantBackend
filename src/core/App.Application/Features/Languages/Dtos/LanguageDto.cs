namespace App.Application.Features.Languages.Dtos;

public record LanguageDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? ImageUrl { get; init; }
}
