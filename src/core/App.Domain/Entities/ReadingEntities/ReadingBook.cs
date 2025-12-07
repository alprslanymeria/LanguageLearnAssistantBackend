namespace App.Domain.Entities.ReadingEntities;

public class ReadingBook : BaseEntity<int>
{
    public int ReadingId { get; set; }
    public string Name { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string LeftColor { get; set; } = default!;
    public string SourceUrl { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Reading Reading { get; set; } // FOR ReadingId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<ReadingOldSession> ReadingOldSessions { get; set; } = [];
}
