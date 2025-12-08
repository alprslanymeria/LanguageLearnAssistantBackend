namespace App.Application.Features.Writings.Dtos;

public record UpdateWritingDto
{
    public int Id { get; init; }
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
