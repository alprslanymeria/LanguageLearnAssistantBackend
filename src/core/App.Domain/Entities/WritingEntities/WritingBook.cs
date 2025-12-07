namespace App.Domain.Entities.WritingEntities;

public class WritingBook : BaseEntity<int>
{
    public int WritingId { get; set; }
    public string Name { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string LeftColor { get; set; } = default!;
    public string SourceUrl { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Writing Writing { get; set; } // FOR WritingId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<WritingOldSession> WritingOldSessions { get; set; } = [];
}