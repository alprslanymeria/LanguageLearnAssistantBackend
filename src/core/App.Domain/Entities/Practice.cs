using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;

namespace App.Domain.Entities;

// LANGUAGES AND PRACTICES DEFINED AS A ONE -TO-MANY RELATIONSHIP BUT IN USAGE THEY ACT LIKE MANY-TO-MANY. IT'S MY DECISION.

public class Practice : BaseEntity<int>
{
    public int LanguageId { get; set; }
    public string Name { get; set; } = null!;

    // REFERENCES (PARENTS)
    public required Language Language { get; set; } // FOR LanguageId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<Flashcard> Flashcards { get; set; } = [];
    public ICollection<Listening> Listenings { get; set; } = [];
    public ICollection<Writing> Writings { get; set; } = [];
    public ICollection<Reading> Readings { get; set; } = [];
}
