namespace App.Domain.Entities.WritingEntities;

public class WritingOldSession : AuditableEntity<string>
{
    public int WritingId { get; set; }
    public int WritingBookId { get; set; }
    public decimal Rate { get; set; }

    // REFERENCES (PARENTS)
    public Writing? Writing { get; set; } // FOR WritingId
    public WritingBook? WritingBook { get; set; } // FOR WritingBookId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<WritingSessionRow> WritingSessionRows { get; set; } = [];
}
