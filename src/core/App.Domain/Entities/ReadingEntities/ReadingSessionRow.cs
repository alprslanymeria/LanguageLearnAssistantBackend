namespace App.Domain.Entities.ReadingEntities;

public class ReadingSessionRow : BaseEntity<int>
{
    public string ReadingOldSessionId { get; set; } = null!;
    public string SelectedSentence { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public string AnswerTranslate { get; set; } = null!;
    public decimal Similarity { get; set; }

    // REFERENCES (PARENTS)
    public required ReadingOldSession ReadingOldSession { get; set; } // FOR ReadingOldSessionId
}