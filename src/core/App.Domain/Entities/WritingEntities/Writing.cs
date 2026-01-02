namespace App.Domain.Entities.WritingEntities;

public class Writing : BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public int LanguageId { get; set; }
    public int PracticeId { get; set; }

    // REFERENCES (PARENTS)
    public required Language Language { get; set; } // FOR LanguageId
    public required Practice Practice { get; set; } // FOR PracticeId


    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<WritingBook> WritingBooks { get; set; } = [];
    public ICollection<WritingOldSession> WritingOldSessions { get; set; } = [];
}