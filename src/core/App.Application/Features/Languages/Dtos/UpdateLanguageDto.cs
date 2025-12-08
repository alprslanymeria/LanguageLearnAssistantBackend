namespace App.Application.Features.Languages.Dtos;

public record UpdateLanguageDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? ImageUrl { get; init; }
}
