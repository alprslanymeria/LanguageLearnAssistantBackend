namespace App.Domain.Entities.Writing;

public class WritingOldSession : BaseEntity<string>
{
    public int WritingId { get; set; }
    public int WritingBookId { get; set; }
    public decimal Rate { get; set; }
    public DateTime CreatedAt { get; set; } // DEFAULT NOW DEĞERİ VERİLECEK --> FLUENT API 

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Writing Writing { get; set; } // FOR WritingId
    public required WritingBook WritingBook { get; set; } // FOR WritingBookId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<WritingSessionRow> WritingSessionRows { get; set; } = [];
}
