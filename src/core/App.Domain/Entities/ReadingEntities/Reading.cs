namespace App.Domain.Entities.ReadingEntities;

public class Reading : BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public int LanguageId { get; set; }
    public int PracticeId { get; set; }

    // REFERENCES (PARENTS)
    public required Language Language { get; set; } // FOR LanguageId
    public required Practice Practice { get; set; } // FOR PracticeId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ReadingBook> ReadingBooks { get; set; } = [];
    public ICollection<ReadingOldSession> ReadingOldSessions { get; set; } = [];
}