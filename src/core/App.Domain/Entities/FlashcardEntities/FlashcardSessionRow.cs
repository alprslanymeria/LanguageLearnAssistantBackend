namespace App.Domain.Entities.FlashcardEntities;

public class FlashcardSessionRow : BaseEntity<int>
{
    public string FlashcardOldSessionId { get; set; } = null!;
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public bool Status { get; set; }

    // REFERENCES (PARENTS)
    public required FlashcardOldSession FlashcardOldSession { get; set; } // FOR FlashcardOldSessionId
}
