namespace App.Domain.Entities.ListeningEntities;

public class ListeningSessionRow : BaseEntity<int>
{
    public string ListeningOldSessionId { get; set; } = default!;
    public string ListenedSentence { get; set; } = default!;
    public string Answer { get; set; } = default!;
    public decimal Similarity { get; set; }

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required ListeningOldSession ListeningOldSession { get; set; } // FOR ListeningOldSessionId
}
