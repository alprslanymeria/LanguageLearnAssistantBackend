namespace App.Domain.Entities.ListeningEntities;

public class ListeningCategory : BaseEntity<int>
{
    public int ListeningId { get; set; }
    public string Name { get; set; } = null!;

    // REFERENCES (PARENTS)
    public required Listening Listening { get; set; } // FOR ListeningId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ListeningOldSession> ListeningOldSessions { get; set; } = [];
    public ICollection<DeckVideo> DeckVideos { get; set; } = [];
}
