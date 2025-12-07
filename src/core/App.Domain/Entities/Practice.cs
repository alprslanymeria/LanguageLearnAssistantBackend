using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;

namespace App.Domain.Entities;

public class Practice : BaseEntity<int>
{
    public int LanguageId { get; set; }
    public string Name { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Language Language { get; set; } // FOR LanguageId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<Flashcard> Flashcards { get; set; } = [];
    public ICollection<Listening> Listenings { get; set; } = [];
    public ICollection<Writing> Writings { get; set; } = [];
    public ICollection<Reading> Readings { get; set; } = [];
}
