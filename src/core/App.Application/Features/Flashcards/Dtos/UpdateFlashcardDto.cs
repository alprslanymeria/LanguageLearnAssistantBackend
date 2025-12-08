namespace App.Application.Features.Flashcards.Dtos;

public record UpdateFlashcardDto
{
    public int Id { get; init; }
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
