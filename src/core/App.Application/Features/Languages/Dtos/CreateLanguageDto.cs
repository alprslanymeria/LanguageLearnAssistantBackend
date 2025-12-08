namespace App.Application.Features.Languages.Dtos;

public record CreateLanguageDto
{
    public string Name { get; init; } = default!;
    public string? ImageUrl { get; init; }
}
