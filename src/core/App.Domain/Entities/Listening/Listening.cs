namespace App.Domain.Entities.Listening;

public class Listening : BaseEntity<int>
{
    public string UserId { get; set; } = default!;
    public int LanguageId { get; set; }
    public int PracticeId { get; set; }

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Language Language { get; set; } // FOR LanguageId
    public required Practice Practice { get; set; } // FOR PracticeId


    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<ListeningCategory> ListeningCategories = [];
    public ICollection<ListeningOldSession> ListeningOldSessions = [];
}