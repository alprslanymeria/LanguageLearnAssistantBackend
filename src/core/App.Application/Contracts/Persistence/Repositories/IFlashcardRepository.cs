using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR FLASHCARD ENTITY.
/// </summary>
public interface IFlashcardRepository : IGenericRepository<Flashcard, int>
{

    /// <summary>
    /// GETS ALL FLASHCARDS FOR A USER WITH LANGUAGE AND PRACTICE INCLUDED.
    /// </summary>
    Task<List<Flashcard>> GetByUserIdWithDetailsAsync(string userId);
}
