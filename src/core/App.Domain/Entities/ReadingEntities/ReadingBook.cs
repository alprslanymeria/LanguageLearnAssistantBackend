namespace App.Domain.Entities.ReadingEntities;

public class ReadingBook : BaseEntity<int>
{
    public int ReadingId { get; set; }
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string LeftColor { get; set; } = null!;
    public string SourceUrl { get; set; } = null!;

    // REFERENCES (PARENTS)
    public Reading? Reading { get; set; } // FOR ReadingId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ReadingOldSession> ReadingOldSessions { get; set; } = [];
}
