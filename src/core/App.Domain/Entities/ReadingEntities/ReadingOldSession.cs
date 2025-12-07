namespace App.Domain.Entities.ReadingEntities;

public class ReadingOldSession : BaseEntity<string>
{
    public int ReadingId { get; set; }
    public int ReadingBookId { get; set; }
    public decimal Rate { get; set; }
    public DateTime CreatedAt { get; set; } // DEFAULT NOW DEĞERİ VERİLECEK --> FLUENT API 

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Reading Reading { get; set; } // FOR ReadingId
    public required ReadingBook ReadingBook { get; set; } // FOR ReadingBookId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<ReadingSessionRow> ReadingSessionRows { get; set; } = [];
}