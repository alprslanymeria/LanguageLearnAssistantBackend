namespace App.Domain.Entities.ListeningEntities;

public class DeckVideo : BaseEntity<int>
{
    public int ListeningCategoryId { get; set; }
    public string Correct { get; set; } = null!;
    public string SourceUrl { get; set; } = null!;

    // REFERENCES (PARENTS)
    public ListeningCategory? ListeningCategory { get; set; } // FOR ListeningCategoryId
}
