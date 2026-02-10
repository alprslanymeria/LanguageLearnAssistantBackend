namespace App.Domain.Entities.FlashcardEntities;

public class FlashcardOldSession : AuditableEntity<string>
{
    public int FlashcardId { get; set; }
    public int FlashcardCategoryId { get; set; }
    public decimal Rate { get; set; }

    // REFERENCES (PARENTS)
    public Flashcard? Flashcard { get; set; } // FOR FlashcardId
    public FlashcardCategory? FlashcardCategory { get; set; } // FOR FlashcardCategoryId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<FlashcardSessionRow> FlashcardSessionRows { get; set; } = [];
}
