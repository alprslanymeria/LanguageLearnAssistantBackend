namespace App.Domain.Entities.Listening;

public class ListeningCategory : BaseEntity<int>
{
    public int ListeningId { get; set; }
    public string Name { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Listening Listening { get; set; } // FOR ListeningId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<ListeningOldSession> ListeningOldSessions { get; set; } = [];
    public ICollection<DeckVideo> DeckVideos { get; set; } = [];
}
