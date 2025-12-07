namespace App.Domain.Entities.FlashcardEntities;

public class FlashcardSessionRow : BaseEntity<int>
{
    public string FlashcardOldSessionId { get; set; } = default!;
    public string Question { get; set; } = default!;
    public string Answer { get; set; } = default!;
    public Boolean Status { get; set; }

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required FlashcardOldSession FlashcardOldSession { get; set; } // FOR FlashcardOldSessionId
}
