namespace App.Application.Features.Listenings.Dtos;

public record CreateListeningDto
{
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
