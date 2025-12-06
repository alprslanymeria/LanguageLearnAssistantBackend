namespace App.Domain.Entities.Listening;

public class ListeningOldSession : BaseEntity<string>
{
    public int ListeningId { get; set; }
    public int ListeningCategoryId { get; set; }
    public decimal Rate { get; set; }
    public DateTime CreatedAt { get; set; } // DEFAULT NOW DEĞERİ VERİLECEK --> FLUENT API

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Listening Listening { get; set; } // FOR ListeningId
    public required ListeningCategory ListeningCategory { get; set; } // FOR ListeningCategoryId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<ListeningSessionRow> ListeningSessionRows { get; set; } = [];
}
