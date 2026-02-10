namespace App.Domain.Entities.ListeningEntities;

public class Listening : BaseEntity<int>
{
    public string UserId { get; set; } = null!;
    public int LanguageId { get; set; }
    public int PracticeId { get; set; }

    // REFERENCES (PARENTS)
    public Language? Language { get; set; } // FOR LanguageId
    public Practice? Practice { get; set; } // FOR PracticeId


    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ListeningCategory> ListeningCategories { get; set; } = [];
    public ICollection<ListeningOldSession> ListeningOldSessions { get; set; } = [];
}
