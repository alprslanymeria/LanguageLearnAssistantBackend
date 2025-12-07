namespace App.Domain.Entities.FlashcardEntities;

public class DeckWord : BaseEntity<int>
{
    public int FlashcardCategoryId { get; set; }
    public string Question { get; set; } = default!;
    public string Answer { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required FlashcardCategory FlashcardCategory { get; set; } // FOR FlashcardCategoryId
}