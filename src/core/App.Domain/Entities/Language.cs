namespace App.Domain.Entities;

public class Language : BaseEntity<int>
{
    public string Name { get; set; } = default!; // THIS PROPERTY SEEMS LIKE NULL FOR NOW, BUT EF WILL FILL IT AT RUNTIME; DON'T WORRY ABOUT THE WARNING.
    public string? ImageUrl { get; set; }

    // REFERANS VERDİKLERİ (CHILD'LARI)
    public ICollection<Practice> Practices { get; set; } = [];
    public ICollection<Flashcard.Flashcard> Flashcards { get; set; } = [];
    public ICollection<Listening.Listening> Listenings { get; set; } = [];
    public ICollection<Writing.Writing> Writings { get; set; } = [];
    public ICollection<Reading.Reading> Readings { get; set; } = [];
}

