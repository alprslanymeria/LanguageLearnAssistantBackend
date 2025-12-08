namespace App.Application.Features.Practices.Dtos;

public record CreatePracticeDto
{
    public int LanguageId { get; init; }
    public string Name { get; init; } = default!;
}
