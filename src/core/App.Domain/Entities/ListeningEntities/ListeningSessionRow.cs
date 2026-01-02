namespace App.Domain.Entities.ListeningEntities;

public class ListeningSessionRow : BaseEntity<int>
{
    public string ListeningOldSessionId { get; set; } = null!;
    public string ListenedSentence { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public decimal Similarity { get; set; }

    // REFERENCES (PARENTS)
    public required ListeningOldSession ListeningOldSession { get; set; } // FOR ListeningOldSessionId
}
