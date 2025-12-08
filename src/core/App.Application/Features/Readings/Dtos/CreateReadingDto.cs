namespace App.Application.Features.Readings.Dtos;

public record CreateReadingDto
{
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
