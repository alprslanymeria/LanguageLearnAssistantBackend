namespace App.Domain.Entities.Flashcard;

public class FlashcardOldSession : BaseEntity<string>
{
    public int FlashcardId { get; set; }
    public int FlashcardCategoryId { get; set; }
    public decimal Rate { get; set; }

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Flashcard Flashcard { get; set; } // FOR FlashcardId
    public required FlashcardCategory FlashcardCategory { get; set; } // FOR FlashcardCategoryId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<FlashcardSessionRow> FlashcardSessionRows { get; set; } = [];
}