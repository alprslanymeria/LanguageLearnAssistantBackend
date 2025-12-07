namespace App.Domain.Entities.FlashcardEntities;

public class Flashcard : BaseEntity<int>
{
    public string UserId { get; set; } = default!;
    public int LanguageId { get; set; }
    public int PracticeId { get; set; }

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Language Language { get; set; } // FOR LanguageId
    public required Practice Practice { get; set; } // FOR PracticeId


    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<FlashcardCategory> FlashcardCategories = [];
    public ICollection<FlashcardOldSession> FlashcardOldSessions = [];
}
