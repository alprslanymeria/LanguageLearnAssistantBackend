using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IFlashcardRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(Flashcard entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Flashcard?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE FLASHCARD IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(Flashcard entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE FLASHCARD FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES A FLASHCARD BY ITS PRACTICE ID, USER ID, AND LANGUAGE ID.
    /// </summary>
    Task<Flashcard?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId);
}
