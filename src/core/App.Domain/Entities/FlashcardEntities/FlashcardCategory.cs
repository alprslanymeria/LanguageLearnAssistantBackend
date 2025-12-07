namespace App.Domain.Entities.FlashcardEntities;

public class FlashcardCategory : BaseEntity<int>
{
    public int FlashcardId { get; set; }
    public string Name { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Flashcard Flashcard { get; set; } // FOR FlashcardId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<FlashcardOldSession> FlashcardOldSessions { get; set; } = [];
    public ICollection<DeckWord> DeckWords { get; set; } = [];
}
