namespace App.Application.Features.Flashcards.Dtos;

public record CreateFlashcardDto
{
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
    public int PracticeId { get; init; }
}
