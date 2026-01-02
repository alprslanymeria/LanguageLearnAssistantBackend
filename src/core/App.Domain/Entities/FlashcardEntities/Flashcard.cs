namespace App.Domain.Entities.FlashcardEntities;

public class Flashcard : BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public int LanguageId { get; set; }
    public int PracticeId { get; set; }

    // REFERENCES (PARENTS)
    public required Language Language { get; set; } // FOR LanguageId
    public required Practice Practice { get; set; } // FOR PracticeId


    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<FlashcardCategory> FlashcardCategories { get; set; } = [];
    public ICollection<FlashcardOldSession> FlashcardOldSessions { get; set; } = [];
}
