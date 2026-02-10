namespace App.Domain.Entities.FlashcardEntities;

public class FlashcardCategory : BaseEntity<int>
{
    public int FlashcardId { get; set; }
    public string Name { get; set; } = null!;

    // REFERENCES (PARENTS)
    public Flashcard? Flashcard { get; set; } // FOR FlashcardId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<FlashcardOldSession> FlashcardOldSessions { get; set; } = [];
    public ICollection<DeckWord> DeckWords { get; set; } = [];
}
