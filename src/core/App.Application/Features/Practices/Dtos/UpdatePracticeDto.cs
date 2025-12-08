namespace App.Application.Features.Practices.Dtos;

public record UpdatePracticeDto
{
    public int Id { get; init; }
    public int LanguageId { get; init; }
    public string Name { get; init; } = default!;
}
