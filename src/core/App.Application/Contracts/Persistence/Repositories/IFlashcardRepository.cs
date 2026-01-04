using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD ENTITY.
/// </summary>
public interface IFlashcardRepository
{

    /// <summary>
    /// GETS ALL FLASHCARDS FOR A USER WITH LANGUAGE AND PRACTICE INCLUDED.
    /// </summary>
    Task<List<Flashcard>> GetByUserIdWithDetailsAsync(string userId);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(Flashcard entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Flashcard?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE FLASHCARD IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    Flashcard Update(Flashcard entity);

    /// <summary>
    /// REMOVES THE FLASHCARD FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(Flashcard entity);
}
