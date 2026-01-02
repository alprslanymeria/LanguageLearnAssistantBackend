namespace App.Domain.Entities.WritingEntities;

public class WritingSessionRow : BaseEntity<int>
{
    public string WritingOldSessionId { get; set; } = null!;
    public string SelectedSentence { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public string AnswerTranslate { get; set; } = null!;
    public decimal Similarity { get; set; }

    // REFERENCES (PARENTS)
    public required WritingOldSession WritingOldSession { get; set; } // FOR WritingOldSessionId
}