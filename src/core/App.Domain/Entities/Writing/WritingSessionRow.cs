namespace App.Domain.Entities.Writing;

public class WritingSessionRow : BaseEntity<int>
{
    public string WritingOldSessionId { get; set; } = default!;
    public string SelectedSentence { get; set; } = default!;
    public string Answer { get; set; } = default!;
    public string AnswerTranslate { get; set; } = default!;
    public decimal Similarity { get; set; }

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required WritingOldSession WritingOldSession { get; set; } // FOR WritingOldSessionId
}