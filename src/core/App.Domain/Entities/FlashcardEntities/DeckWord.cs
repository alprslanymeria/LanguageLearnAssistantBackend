namespace App.Domain.Entities.FlashcardEntities;

public class DeckWord : BaseEntity<int>
{
    public int FlashcardCategoryId { get; set; }
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;

    // REFERENCES (PARENTS)
    public FlashcardCategory? FlashcardCategory { get; set; } // FOR FlashcardCategoryId
}
