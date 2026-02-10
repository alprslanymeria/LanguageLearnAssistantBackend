namespace App.Domain.Entities.WritingEntities;

public class WritingBook : BaseEntity<int>
{
    public int WritingId { get; set; }
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string LeftColor { get; set; } = null!;
    public string SourceUrl { get; set; } = null!;

    // REFERENCES (PARENTS)
    public Writing? Writing { get; set; } // FOR WritingId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<WritingOldSession> WritingOldSessions { get; set; } = [];
}
