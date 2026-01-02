namespace App.Domain.Entities.ListeningEntities;

public class ListeningOldSession : AuditableEntity<string>
{
    public int ListeningId { get; set; }
    public int ListeningCategoryId { get; set; }
    public decimal Rate { get; set; }

    // REFERENCES (PARENTS)
    public required Listening Listening { get; set; } // FOR ListeningId
    public required ListeningCategory ListeningCategory { get; set; } // FOR ListeningCategoryId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ListeningSessionRow> ListeningSessionRows { get; set; } = [];
}
