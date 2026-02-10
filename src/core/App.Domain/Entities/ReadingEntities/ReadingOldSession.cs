namespace App.Domain.Entities.ReadingEntities;

public class ReadingOldSession : AuditableEntity<string>
{
    public int ReadingId { get; set; }
    public int ReadingBookId { get; set; }
    public decimal Rate { get; set; }

    // REFERENCES (PARENTS)
    public Reading? Reading { get; set; } // FOR ReadingId
    public ReadingBook? ReadingBook { get; set; } // FOR ReadingBookId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ReadingSessionRow> ReadingSessionRows { get; set; } = [];
}
