namespace App.Application.Features.Writings.Dtos;

public record WritingDto
{
    public int Id { get; init; }
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
    public string? LanguageName { get; init; }
    public string? PracticeName { get; init; }
}
