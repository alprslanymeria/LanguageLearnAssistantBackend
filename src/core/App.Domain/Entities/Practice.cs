namespace App.Domain.Entities;

public class Practice
{
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; } = default!;

    // REFERANS ALDIKLARI (PARENT'LARI)
    public required Language Language { get; set; } // FOR LanguageId

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<Flashcard.Flashcard> Flashcards { get; set; } = [];
    public ICollection<Listening.Listening> Listenings { get; set; } = [];
    public ICollection<Writing.Writing> Writings { get; set; } = [];
    public ICollection<Reading.Reading> Readings { get; set; } = [];
}
