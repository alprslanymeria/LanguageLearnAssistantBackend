namespace App.Application.Features.Practices.Dtos;

public record PracticeDto
{
    public int Id { get; init; }
    public int LanguageId { get; init; }
    public string Name { get; init; } = default!;
    public string? LanguageName { get; init; }
}
