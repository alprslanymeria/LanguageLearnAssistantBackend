namespace App.Application.Features.Listenings.Dtos;

public record UpdateListeningDto
{
    public int Id { get; init; }
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
