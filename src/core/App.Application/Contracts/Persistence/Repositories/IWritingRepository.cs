using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IWritingRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(Writing entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Writing?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE WRITING ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(Writing entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE WRITING ENTITY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES A WRITING BY ITS PRACTICE ID, USER ID, AND LANGUAGE ID.
    /// </summary>
    Task<Writing?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId);
}
