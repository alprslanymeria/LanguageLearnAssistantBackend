namespace App.Application.Features.Writings.Dtos;

public record CreateWritingDto
{
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
