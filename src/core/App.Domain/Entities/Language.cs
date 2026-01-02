using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;

namespace App.Domain.Entities;

// LANGUAGES AND PRACTICES DEFINED AS A ONE -TO-MANY RELATIONSHIP BUT IN USAGE THEY ACT LIKE MANY-TO-MANY. IT'S MY DECISION.

public class Language : BaseEntity<int>
{
    public string Name { get; set; } = null!; // THIS PROPERTY SEEMS LIKE NULL FOR NOW, BUT EF WILL FILL IT AT RUNTIME; DON'T WORRY ABOUT THE WARNING.
    public string? ImageUrl { get; set; }

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<Practice> Practices { get; set; } = [];
    public ICollection<Flashcard> Flashcards { get; set; } = [];
    public ICollection<Listening> Listenings { get; set; } = [];
    public ICollection<Writing> Writings { get; set; } = [];
    public ICollection<Reading> Readings { get; set; } = [];
}

